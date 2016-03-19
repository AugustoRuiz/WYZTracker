using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WYZTracker
{
    public class SongBinProvider
    {
        static int[] longitudPuntillo = { -1, 1, 2, 4 };

        public static Song LoadSong(string fileName)
        {
            return null;
        }

        public static byte[] GenerateSong(Song song)
        {
            List<byte> bytes = new List<byte>();
            Pattern current;
            ChannelNote note;
            int noteLen;
            bool lastNoteSilence;
            string[] lastInstrument = new string[song.Channels];
            EnvelopeData[] lastEnvelopes = new EnvelopeData[song.Channels];
            int[] loopLocations = new int[song.Channels + 1];

            int d;
            int dSeminote;
            int o;

            bytes.Add(song.Tempo);

            byte headerByte = (byte)(song.Looped ? 0x01 : 0x00);
            headerByte |= (byte)((song.FxChannel & 0x7) << 1);
            headerByte |= (byte)((song.Channels & 0x7) << 4);
            bytes.Add(headerByte);

            // Dos bytes reservados (de momento).
            bytes.Add(0); bytes.Add(0);

            // Metemos los bytes de offset de loops.
            int offsetLoopLocations = bytes.Count;
            bytes.AddRange(new byte[2 * (song.Channels + 1)]);

            for (int currentChannel = 0; currentChannel < song.Channels; currentChannel++)
            {
                lastInstrument[currentChannel] = string.Empty;
                lastEnvelopes[currentChannel] = null;
            }

            List<string> notesWithTempoModifier = new List<string>();
            for (int currentChannel = 0; currentChannel < song.Channels; currentChannel++)
            {
                noteLen = 0;
                lastNoteSilence = false;
                o = 0;
                d = 0;
                dSeminote = 0;

                sbyte? volModifier = null;
                sbyte tempoModifier;
                List<byte> bytesPatron = new List<byte>();
                bool calculatingFirstNoteOffset = false;
                int firstNoteOffsetInLoopToPattern = -1;

                for (int currentPattern = 0; currentPattern < song.PlayOrder.Count; currentPattern++)
                {
                    current = song.Patterns[song.PlayOrder[currentPattern]];

                    if (song.Looped && currentPattern == song.LoopToPattern)
                    {
                        calculatingFirstNoteOffset = true;
                        firstNoteOffsetInLoopToPattern = 0;
                    }

                    for (int currentLine = 0; currentLine < current.Length; currentLine++)
                    {
                        ChannelLine line = current.Lines[currentLine];
                        note = line.Notes[currentChannel];
                        tempoModifier = 0;
                        if (note.HasValue)
                        {
                            if (noteLen > 0)
                            {
                                noteLen = addNote(bytesPatron, noteLen, lastNoteSilence, d, dSeminote, o);
                            }
                            if (calculatingFirstNoteOffset && song.Looped)
                            {
                                loopLocations[currentChannel] = bytesPatron.Count; // + bytesAntesDeInstrumento.Count;
                                firstNoteOffsetInLoopToPattern += currentLine;
                                calculatingFirstNoteOffset = false;
                            }
                        }

                        if (note.HasOctave)
                        {
                            o = note.Octave - 2;
                        }

                        if (note.HasNote)
                        {
                            dSeminote = 0;

                            if (note.Note == 'P')
                            {
                                lastNoteSilence = true;
                            }
                            else
                            {
                                lastNoteSilence = false;
                                d = getNote(note.Note);
                            }
                            volModifier = note.VolModifier;
                            string key = string.Format("{0}_{1}", currentPattern, currentLine);
                            if (!notesWithTempoModifier.Contains(key))
                            {
                                tempoModifier = line.TempoModifier;
                                if (tempoModifier != 0)
                                {
                                    notesWithTempoModifier.Add(key);
                                }
                            }
                            else
                            {
                                tempoModifier = 0;
                            }
                        }

                        if (note.HasSeminote)
                        {
                            if (note.Seminote == '+')
                                dSeminote = 1;
                            else
                                dSeminote = -1;
                        }
                        if (note.HasInstrument || note.VolModifier.HasValue || tempoModifier != 0)
                        {
                            if (note.VolModifier.HasValue || note.Instrument != lastInstrument[currentChannel] ||
                                (note.IsSawtooth && !EnvelopeData.Compare(note.EnvData, lastEnvelopes[currentChannel])))
                            {
                                if (note.Instrument.ToUpper() == "R")
                                {
                                    bytesPatron.Add(0xbf);
                                    bytesPatron.Add(getSawtoothParamsByte(note));
                                    lastEnvelopes[currentChannel] = note.EnvData;
                                }
                                else
                                {
                                    bytesPatron.Add(0x3f);
                                    bytesPatron.Add(note.HasInstrument ? byte.Parse(note.Instrument) : byte.Parse(lastInstrument[currentChannel]));
                                    byte tModifier = tempoModifier == -1 ? (byte)(1 << 5) : tempoModifier == 1 ? (byte)(1 << 6) : (byte)0;
                                    byte modifiers = (byte)((note.VolModifier.GetValueOrDefault() & 0x1F) | tModifier);
                                    bytesPatron.Add(modifiers);
                                    lastEnvelopes[currentChannel] = null;
                                }

                                if (note.HasInstrument)
                                {
                                    lastInstrument[currentChannel] = note.Instrument;
                                }
                            }
                        }
                        noteLen++;
                    }

                    // Calculamos la longitud de la última nota haciendo que añada la longitud del canal actual completo 
                    // si en este canal no hay notas, a la espera de encontrar una nota en este canal.
                    if (calculatingFirstNoteOffset)
                    {
                        firstNoteOffsetInLoopToPattern += current.Length;
                    }
                }
                if (noteLen > 0 && bytesPatron.Count > 0)
                {
                    noteLen = addNote(bytesPatron,
                        noteLen + ((song.Looped && firstNoteOffsetInLoopToPattern >= 0) ? firstNoteOffsetInLoopToPattern : 0),
                        lastNoteSilence, d, dSeminote, o);
                }

                // Si no se ha encontrado aún una nota a la que saltar cuando hay loop, entonces el salto del loop es 
                // al final del canal.
                if (calculatingFirstNoteOffset)
                {
                    loopLocations[currentChannel] = bytesPatron.Count;
                }

                if (bytesPatron.Count == 0)
                {
                    bytesPatron.Add(0xC1);
                }
                bytes.AddRange(bytesPatron);
                bytes.Add(0x00); // Fin de canal.
            }

            // Canal de FX!!!
            List<byte> bytesFX = new List<byte>();
            int currentFx = int.MinValue;

            noteLen = 0;
            bool calculatingFirstFxOffset = false;
            int firstFxOffsetInLoopToPattern = -1;
            for (int currentPattern = 0; currentPattern < song.PlayOrder.Count; currentPattern++)
            {
                if (song.Looped && currentPattern == song.LoopToPattern)
                {
                    calculatingFirstFxOffset = true;
                    firstFxOffsetInLoopToPattern = 0;
                }
                current = song.Patterns[song.PlayOrder[currentPattern]];
                for (int currentNote = 0; currentNote < current.Length; currentNote++)
                {
                    // Ver cómo se tiene que generar los FX.
                    if (current.Lines[currentNote].Fx != int.MinValue && noteLen > 0)
                    {
                        noteLen = addFX(bytesFX, noteLen, currentFx, song);
                    }
                    if (current.Lines[currentNote].Fx != int.MinValue)
                    {
                        currentFx = current.Lines[currentNote].Fx;

                        if (song.Looped && calculatingFirstFxOffset)
                        {
                            calculatingFirstFxOffset = false;
                            firstFxOffsetInLoopToPattern += currentNote;
                            loopLocations[song.Channels] = bytesFX.Count;
                        }
                    }
                    noteLen++;
                }
                if (calculatingFirstFxOffset)
                {
                    firstFxOffsetInLoopToPattern += current.Length;
                }
            }

            if (noteLen > 0 && currentFx >= 0)
            {
                noteLen = addFX(bytesFX,
                    noteLen + ((song.Looped && firstFxOffsetInLoopToPattern >= 0) ? firstFxOffsetInLoopToPattern : 0),
                    currentFx,
                    song);
            }

            if (calculatingFirstFxOffset)
            {
                loopLocations[song.Channels] = bytesFX.Count;
            }

            if (bytesFX.Count == 0)
            {
                bytesFX.Add(0xC1);
            }

            bytes.AddRange(bytesFX);

            bytes.Add(0x00); // Fin de canal.

            bytes.Add(0x00); // Fin de canción. (¿¿Hace falta??)

            for (int c = 0; c <= song.Channels; ++c)
            {
                bytes[offsetLoopLocations + 2 * c] = (byte)(loopLocations[c] & 0x00FF);
                bytes[offsetLoopLocations + (2 * c) + 1] = (byte)((loopLocations[c] & 0x0FF00) >> 8);
            }

            return bytes.ToArray();
        }

        private static byte getSawtoothParamsByte(ChannelNote note)
        {
            byte result = 0x0;

            result |= (note.EnvData.ActiveFrequencies) ? (byte)0x1 : (byte)0x0;

            switch (note.EnvData.FrequencyRatio)
            {
                case 2:
                    break;
                case 4:
                    result |= (byte)(0x1) << 1;
                    break;
                case 8:
                    result |= (byte)(0x2) << 1;
                    break;
                case 16:
                    result |= (byte)(0x3) << 1;
                    break;
            }

            switch (note.EnvData.Style)
            {
                case 8:
                    break;
                case 10:
                    result |= (byte)(0x1) << 3;
                    break;
                case 12:
                    result |= (byte)(0x2) << 3;
                    break;
                case 14:
                    result |= (byte)(0x3) << 3;
                    break;
            }


            return result;
        }

        private static int addFX(List<byte> bytesFX, int noteLen, int currentFx, Song song)
        {
            int l;
            int w = 0;

            if (noteLen > 0)
            {
                l = getLength(ref noteLen);
                if (currentFx != int.MinValue)
                {
                    bytesFX.Add(0x7F);
                    w = 0x40 * l + currentFx;
                }
                else
                {
                    w = 0x40 * l + 1;
                }
                bytesFX.Add((byte)w);

                while (noteLen > 0)
                {
                    if (noteLen >= longitudPuntillo[l])
                    {
                        while (noteLen >= longitudPuntillo[l])
                        {
                            w = 0x40 * l + 0x3E;
                            bytesFX.Add((byte)w);
                            noteLen -= longitudPuntillo[l];
                        }
                    }
                    else
                    {
                        l = getLength(ref noteLen);
                        bytesFX.Add((byte)(0x40 * l + 1));
                    }
                }
            }

            return noteLen;
        }

        private static int addNote(List<byte> bytes, int noteLen, bool lastNoteSilence, int d, int dSeminote, int o)
        {
            int l;
            int w;

            if (noteLen > 0)
            {
                l = getLength(ref noteLen);
                if (lastNoteSilence)
                {
                    w = 0x40 * l + 1;
                }
                else
                {
                    w = 12 * o + 0x40 * l + (d + dSeminote) + 2;
                }
                bytes.Add((byte)w);

                int currentLength = l;
                while (noteLen > 0)
                {
                    int longPuntillo = longitudPuntillo[currentLength];
                    while (noteLen >= longPuntillo)
                    {
                        w = 0x40 * currentLength + 0x3E;
                        bytes.Add((byte)w);
                        noteLen -= longPuntillo;
                    }
                    currentLength--;
                }
            }
            return noteLen;
        }

        private static int getNote(char p)
        {
            int value = 0;

            switch (p)
            {
                case 'C':
                    value = 0;
                    break;
                case 'D':
                    value = 2;
                    break;
                case 'E':
                    value = 4;
                    break;
                case 'F':
                    value = 5;
                    break;
                case 'G':
                    value = 7;
                    break;
                case 'A':
                    value = 9;
                    break;
                case 'B':
                    value = 11;
                    break;
            }

            return value;
        }

        private static int getFxLength(ref int noteLen)
        {
            int value = 0;

            if (noteLen >= 8)
            {
                value = 3;
                noteLen -= 8;
            }
            else if (noteLen >= 4)
            {
                value = 2;
                noteLen -= 4;
            }
            else if (noteLen >= 2)
            {
                value = 1;
                noteLen -= 2;
            }
            else if (noteLen == 1)
            {
                value = 0;
                noteLen--;
            }

            return value;
        }

        private static int getLength(ref int noteLen)
        {
            int value = 0;

            if (noteLen >= 8)
            {
                value = 3;
                noteLen -= 8;
            }
            else if (noteLen >= 4)
            {
                value = 2;
                noteLen -= 4;
            }
            else if (noteLen >= 2)
            {
                value = 1;
                noteLen -= 2;
            }
            else if (noteLen == 1)
            {
                value = 0;
                noteLen--;
            }

            return value;
        }

        public static string GenerateInstrumentsAndFX(Song song)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTablaPautas = generarTablaPautas(song);
            StringBuilder sbTablaEfectos = generarTablaEfectos(song);

            sb.AppendLine();
            sb.AppendLine(";Pautas (instrumentos)");
            for (i = 0; i < song.Instruments.Count; i++)
            {
                Instrument inst = song.Instruments[i];
                if (inst.ID != "R")
                {
                    sb.AppendFormat(";Instrumento '{0}'", inst.Name);
                    sb.AppendLine();
                    string nombrePauta = string.Format("PAUTA_{0}", inst.ID);

                    sb.AppendFormat("{0}:\tDB\t", nombrePauta);

                    for (int j = 0; j < inst.Volumes.Length; j++)
                    {
                        sb.AppendFormat("{0},{1},", inst.Volumes[j], inst.PitchModifiers[j]);
                    }
                    if (inst.Looped)
                    {
                        sb.AppendFormat("{0}", 0x80 + inst.Volumes.Length - inst.LoopStart);
                    }
                    else
                    {
                        sb.AppendFormat("0,{0}", 0x80 + 1);
                    }
                    sb.AppendLine();
                }
            }

            sb.AppendLine();
            sb.AppendLine(";Efectos");
            for (i = 0; i < song.Effects.Count; i++)
            {
                Effect eff = song.Effects[i];
                sb.AppendFormat(";Efecto '{0}'", eff.Name);
                sb.AppendLine();

                string nombreEfecto = string.Format("SONIDO{0}", eff.ID);
                sb.AppendFormat("{0}:\tDB\t", nombreEfecto);

                for (int k = 0; k < eff.Volumes.Length; k++)
                {
                    bool hasEnv = ((eff.Noises[k] & 0x80) == 0x80);
                    sb.AppendFormat("{0},{1},{2},", (eff.Frequencies[k] & 0xFF), ((eff.Frequencies[k] & 0x0F00) >> 4) + eff.Volumes[k], eff.Noises[k]);
                    if (hasEnv)
                    {
                        sb.AppendFormat("{0},{1},{2},", eff.EnvFreqs[k] & 0xFF, (eff.EnvFreqs[k] & 0x0F00) >> 8, eff.EnvTypes[k]);
                    }
                }
                sb.Append("255");
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine(";Frecuencias para las notas");
            sb.AppendLine("DATOS_NOTAS: DW 0,0");
            for (i = 0; i < 6; i++)
            {
                int baseIdx = (i * 10) + 2;
                sb.Append("DW ");
                for (int j = 0; j < 10; j++)
                {
                    sb.AppendFormat("{0},", song.Frequencies[baseIdx + j]);
                }
                sb.Length--;
                sb.AppendLine();
            }

            sb.Insert(0, sbTablaEfectos.ToString());
            sb.Insert(0, sbTablaPautas.ToString());

            return sb.ToString();
        }

        private static StringBuilder generarTablaEfectos(Song song)
        {
            StringBuilder sbTablaEfectos = new StringBuilder();

            sbTablaEfectos.AppendLine();
            sbTablaEfectos.AppendLine("; Tabla de efectos");
            sbTablaEfectos.Append("TABLA_SONIDOS: DW ");

            if (song.Effects.Count == 0)
            {
                sbTablaEfectos.Append("0,");
            }
            else
            {
                Dictionary<int, Effect> effectTable = new Dictionary<int, Effect>();
                int maxId = int.MinValue;

                foreach (Effect e in song.Effects)
                {
                    effectTable.Add(e.ID, e);
                    if (e.ID > maxId)
                    {
                        maxId = e.ID;
                    }
                }

                for (int fxId = 0; fxId <= maxId; fxId++)
                {
                    if (effectTable.ContainsKey(fxId))
                    {
                        string nombreEfecto = string.Format("SONIDO{0}", fxId);
                        sbTablaEfectos.AppendFormat("{0},", nombreEfecto);
                    }
                    else
                    {
                        sbTablaEfectos.Append("0,");
                    }
                }
            }

            sbTablaEfectos.Length--;
            sbTablaEfectos.AppendLine();

            return sbTablaEfectos;
        }

        private static StringBuilder generarTablaPautas(Song song)
        {
            StringBuilder sbTablaPautas = new StringBuilder();

            sbTablaPautas.AppendLine();
            sbTablaPautas.AppendLine("; Tabla de instrumentos");
            sbTablaPautas.Append("TABLA_PAUTAS: DW ");

            if (song.Instruments.Count == 0)
            {
                sbTablaPautas.Append("0,");
            }
            else
            {
                Dictionary<int, Instrument> instrumentTable = new Dictionary<int, Instrument>();

                int maxId = int.MinValue;
                foreach (Instrument i in song.Instruments)
                {
                    int id;
                    if (int.TryParse(i.ID, out id))
                    {
                        instrumentTable.Add(id, i);
                        if (id > maxId)
                        {
                            maxId = id;
                        }
                    }
                }

                for (int instrumentId = 0; instrumentId <= maxId; instrumentId++)
                {
                    if (instrumentTable.ContainsKey(instrumentId))
                    {
                        string nombrePauta = string.Format("PAUTA_{0}", instrumentId);
                        sbTablaPautas.AppendFormat("{0},", nombrePauta);
                    }
                    else
                    {
                        sbTablaPautas.Append("0,");
                    }
                }
            }

            sbTablaPautas.Length--;
            sbTablaPautas.AppendLine();

            return sbTablaPautas;
        }
    }
}
