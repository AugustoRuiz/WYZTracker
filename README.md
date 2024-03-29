# WYZTracker Manual
![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/banner.png "WYZTracker Logo")
## Introduction
WYZTracker is a tool developed by Augusto Ruiz, from Retroworks, that can be used to compose music for [WYZ's Player](https://github.com/AugustoRuiz/WYZPlayer).

### An important note:
WYZTracker requires OpenAL. [Windows Installer is here](https://www.openal.org/downloads/oalinst.zip).

New player features:
* Song can loop to an specific pattern.
* Drums can use envelopes.
* Tempo modifiers.
* Support for MIDI keyboards.

[Download here!](https://github.com/AugustoRuiz/WYZTracker/releases/download/2.0.1.20/WYZTracker.2.0.1.20.7z)  
[Keys reference here!!](#keyreference)

## A first look at the application
This is the window that will appear when you first run the application:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/principal.png "Main screen")

You can see at the top the Menu Bar, then the Tool Bar, and below you can see from left to right:

* Pattern order editor
* Song Properties editor
* Envelope settings (for Sawtooth instrument)
* Instrument/FX editor
Below, you can see the Pattern Editor. The current pattern editor will allow you to edit the pattern that is selected in the pattern order editor.

### I didn't see that the first time!

Well, that's true. Actually, the first thing you see when the application starts for the first time is something like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/extension.png "Extension association setup")

This screen is pretty obvious, it will check whether .wyz files are associated with WYZTracker, and if not (which will probably will be the case the first time you run the application), will kindly ask you if you want to associate the .wyz file extension to WYZTracker, so that double clicking any .wyz file will open a new instance of the tracker with the specified file.

## Menu Bar

Now, let's see what we can do with the menu bar:
## File Menu

The File Menu looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/menuArchivo.png "File menu")

And what you can do is the following:
* `New`: Create a new song. Be warned, WYZTracker only handles one file at a time, so the current one will be closed.
* `Open`: Open an existing song.
* `Save`: Saves the current song. If it wasn't saved before, it will ask you for a file name.
* `Save As...`: Saves the current song, letting you specify a new name.
* `Export`: Generate the .mus and .asm files in order to include them in your z80 project.
* `Export Audio...`: Generate a wav or ogg file with the current song.
* `Exit`: Exits the application (pretty obvious, huh?)

## Edit Menu

The Edit menu looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/menuEdicion.png "Edit menu")
	
And what you can do is the following:
* `Undo/Redo`: Not implemented yet :(
* `Cut`: Cut the current selected fragment of the current pattern.
* `Copy`: Copy the current selected fragment of the current pattern.
* `Paste`: Paste the previously cut/copied fragment into the current position.
* `Paste as echo`: Paste the previously cut/copied fragment into the current position, but if there is a note already in place, don't overwrite it. It will apply a decrement in volume specified by the toolbar item "Echo decrement".
* `Select All`: Not implemented yet :(

## Instruments Menu

The Instruments menu looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/menuInstrumentos.png "Instruments menu")
	
And what you can do is the following:
* `Import`: Lets you import a set of instruments from a .ins file, or from a .wyz file. Just select the file type in the window that appears, and the file you want to import. It will add the instrument to the current ones. Be careful with the instrument IDs!
* `Export`: Lets you export the current set of instruments into a .ins file.

## Tools Menu

The Tools menu looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/menuHerramientas.png "Tools menu")
	
And what you can do is the following:
* `Options`: Will show you the options screen.
* `Frequency table editor`: Will show you the frequency table editor.
* `Arpeggio editor`: Will show you the arpeggio editor.

## Help Menu

The Help menu looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/menuAyuda.png "Help menu")
	
And what you can do is the following:
* `Online Help`: Will take you right here!
* `About`: Will show you a nice about screen.

## Tool Bar

The tool bar has some convenient buttons in it that will allow you to do the following:

* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filenew.png "New")  Create a new file.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/fileopen.png "Open") Open an existing file.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filesave.png "Save") Save the current file.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/player_end.png "Play") Play the current song from the start.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/player_play.png "Play from pattern") Play the current song from the current pattern.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/switchuser.png "Loop pattern") Play the current pattern, repeating it until stopped.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/player_stop.png "Stop") Stop playing. Pressing stop twice will take you to the beginning of the song.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/addPattern.png "Add pattern") Adds a new pattern to the pattern order list. The new pattern is added after the currently selected, and the pattern Id is the same as the currently selected.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/removePattern.png "Remove pattern") Removes the currently selected pattern from the pattern order list. Note this doesn't actually delete the pattern, just removes it from the pattern order list.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/clonePattern.png "Clone pattern") Clones the currently selected pattern, and adds the clone to the pattern order list, below the currently selected one.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/importarMus.png "Import MUS") Imports a .MUS file (binary WYZ Player format). Not implemented yet, will always appear dimmed.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/exportarWYZ.png "Export MUS") Export current song to .MUS/.ASM files. Just like File Menu->Export.

There are also some other controls in the Tool Bar:

* `Base Octave`: The octave that will be used for new notes added with the virtual piano in the Pattern Editor. Can also be incremented pressing F2, and decremented pressing F3.
* `Auto Increment`: The number of spaces the cursor will advance when adding a new note with the virtual piano in the pattern editor. It can be set to zero if we want to advance the cursor manually. Can also be incremented pressing F4, and decremented pressing F5.
* `Highlight`: Set the interval that will be highlighted in the Pattern Editor.
* `Stereo`: Set the output stereo mode for the current song (only valid for listening or audio export, will not affect the MUS export).
* `Echo decrement`: Determines how many levels will the volume decrease when pasting notes as echo.

By the way, all buttons have a tooltip that will be shown if you let your mouse for a while over the button.

## Pattern Order Editor (pattern selector)

The Pattern Order Editor will let you sort the song patterns and move them around in order to create your song structure. Also is the place where you can create a new pattern, and specify how long the current pattern will be. The current pattern that is being edited is selected in this editor too.

Here is what you can do with this editor:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/patternOrderEditor.png "Pattern order editor")

* Pattern list: Click any pattern in the list to select it.
* Up arrow (![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/move_task_up.png "Up arrow")): Move the currently selected pattern up.
* Down arrow (![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/move_task_down.png "Down arrow")): Move the currently selected pattern down.
* Plus (![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/edit_add.png "Plus")): Increase the currently selected pattern ID by one. If the pattern ID doesn't exist, a new pattern will be created.
* Minus (![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/edit_remove.png "Minus")): Decrease the currently selected pattern ID by one.
* Length: Set the length of the currently selected pattern. Be warned that if you decrease the current length, you can actually lose data!

## Song Properties Editor

The song properties editor will let you set the song general properties, such as the song title, tempo, number of channels...

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/songProperties.png "Song properties editor")

The data you can input here is:

* Name: Song title
* Channels: Number of channels (1-8)
* Tempo: Song tempo. The higher the number, the slower the tempo. It represents the number of interrupts that the current note will be played.
* Loop: Indicates if the song will start playing from the beginning once the song finishes or not.
* Machine: Here you can select the AY emulation frequency, whether you want the player to play with MSX/Spectrum frequency (1.77MHz) or CPC frequency (1 MHz). So, you can check how the song will sound in different kind of machines! (you can fiddle with the frequency tables in order to adjust it to your taste).

## Envelope Settings Editor

The envelope settings editor is used to set the current parameters that will be used when adding notes that are going to be played using the Sawtooth instrument.

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/envelopeSettings.png "Envelope settings editor")

The settings map directly to the parameters the AY uses to control the envelopes, so I highly recommend you to play with them and see how they sound combined. You can do this with a new song. Select the Sawtooth instrument in the instrument selector, and start pressing keys from the virtual piano. Change some settings and press some more... And please, try the Active frequencies setting ;)

##Instrument Selector

This is a list of the instruments defined for the current song. You can change the selected instrument, which is the one that will be used when pressing keys of the Virtual Piano. Also, right clicking an instrument will allow you to export that instrument. In order to export all of them at once, use the Menu Bar, or the export button in the tool bar below the instrument selector.

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/ctxtInstrument.png "Instrument selector")

Double clicking an instrument will show you the Instrument Editor.

There is a toolbar below the selector that will allow you to create a new instrument, delete the selected one, import instruments, export instruments, show the instrument editor, and display the current instrument name.

You can change the currently selected Instrument quickly by pressing F6 and/or F7.

## Effect Selector

This is a list of effects defined for the current song. Right clicking an effect will allow you to export the current effect. In order to export all of them at once, use the Menu Bar, or the export button in the tool bar below the FX selector.

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/ctxtFx.png "Effect selector")

Double clicking an effect will show the Effect Editor.

There is a toolbar below the selector that will allow you to create a new effect, delete the selected one, import effects, export effects, show the effect editor, and display the current effect name.

## Pattern Editor

So, finally! The pattern editor is where you fill the notes that will be played for the current pattern, with which instruments, and for how long... In order to do so easily, you can use the Virtual Piano. Whenever you press a key of the virtual piano, it will add the corresponding note to the currently selected position in the pattern editor.

### Working with the virtual piano

The virtual piano has two octaves. The lower octave starts with the key Z, which is a C note, and all keys in that keyboard row are the D, E, F, G notes and so on. The next row of keys is where the sharp notes lie, so there are some keys that don't have an actual note. For example, C# is the S key. The actual value of the octave is the one selected in the "Base Octave" that appears in the Tool Bar.

If the currently selected position is inside the FX channel, then the virtual piano works differently. It uses the keys 1,2,3,... for the FX, so you don't have to change the FX by selecting it when creating drum sequences.

### MIDI Piano

You can use a MIDI Piano. Be sure to plug it in BEFORE you start WYZTracker.

### Multiple selection

In the pattern editor you can select ranges by selecting and dragging with the left mouse button. That way you can cut, copy or apply modifiers to all notes in the selected range.

The pattern editor has a contextual menu:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/ctxtPatternEditor.png "Pattern editor")

All the items in this contextual menu will work on the selected range of notes. It will allow you to transpose notes, change the octave, or set the instrument to the one you specify.

### Keyboard reference (for Virtual Piano)

Note | Lower octave | Lower octave # | Higher octave | Higher octave #
-----|--------------|----------------|---------------|-----------------
C  | Z |   | Q |
C# |   | S |   | 2
D  | X |   | W |
D# |   | D |   | 3 
E  | C |   | E |
F  | V |   | R
F# |   | G |   | 5
G  | B |   | T |
G# |   | H |   | 6 
A  | N |   | Y |
A# |   | J |   | 7
B  | M |   | U |

Space: Set silence to current note  
Del, Backspace: Delete note  
Return: Play/Stop song  
Escape: Stop song.  
  
Cursor keys: Move around  
PageUp/PageDown/Home/End: Move around (faster)  

F2: Decrease base octave  
F3: Increase base octave  
F4: Decrease auto increment  
F5: Increase auto increment  
F6: Select previous instrument  
F7: Select next instrument  
F8: Remove instrument data from current note

## Instrument Editor
This is the place where you can edit, add, or remove instruments for the current song. It looks like this:

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/instrumentos.png "Instrument editor")

On the left you have the instrument selector. Click on any instrument to show it on the right side. Below the instrument selector, you have a small toolbar where you can:

* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filenew.png "New") Create a new instrument.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/button_cancel.png "Remove") Remove the currently selected instrument. Instrument Sawtooth cannot be deleted.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/fileopen.png "Import") Import instruments from an .ins file or a .wyz file.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filesave.png "Export") Export all instruments.

You can export a single instrument by right-clicking on the instrument and selecting "Export Instrument...".

### Defining an instrument
An instrument is defined by:
* Id: Instrument identifier, a numeric value that is used in the channel lane,
* Name: Instrument name.
* Length: Instrument length.
* Loop: Whether the instrument is looped, and if so, which position does the loop start at.
* Volume levels.
* Octave modifiers.
* Pitch modifiers. 
### Setting the volumes and modifiers
You can set the volumes by left clicking on the odd columns of the editor, and dragging. The checkbox "Lock" can be used to restrict the change of the current value to the column the mouse was over when the drag started.

The modifiers can be:
* Octave modifiers: Right click on a volume, and select the octave modifier using the context menu that appears.
* Pitch modifiers: Accumulative pitch modifier, right click on an even column with the left mouse button.
* Note modifier: Left click on an even column with the right mouse button.
### Testing the instrument
Click the Test button, and it will remain pressed. Now, you can test the instrument by using the Keyboard the same way you would use it to input notes in the Pattern Editor. Clicking the Test button again will release the button, and the Test will be finished.

## Effect editor

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/efectos.png "Effect editor")

On the left you have the effect selector. Click on any effect to show it on the right side. Below the effect selector, you have a small toolbar where you can:

* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filenew.png "New") Create a new effect.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/button_cancel.png "Remove") Remove the currently selected effect.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/fileopen.png "Import") Import effects from an .fx file or a .wyz file.
* ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/filesave.png "Export") Export all effects.

You can export a single effect by right-clicking on the effect and selecting "Export Effect...".

### Defining an effect
An effect is defined by:
* Id: Effect identifier, a numeric value that is used in the FX lane.
* Name: Effect name.
* Length: Effect length.
* Envelope types: ![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/envelopeTypes.png "Export")
* Volume levels.
* Noise levels.
* Frequency levels. 
* Envelope levels.
### Setting the levels
You can set the levels by left clicking on the editor, and dragging. The checkbox "Lock" can be used to restrict the change of the current value to the column the mouse was over when the drag started. The levels are color coded:
* Volume: Green bars
* Noise: Red bars
* Frequency: Yellow bars
* Envelope: Gray bars
### Testing the effect
Click the Test button, and it will remain pressed. Now, you can test the effect by using the Keyboard the same way you would use it to input effects in the Pattern Editor. Clicking the Test button again will release the button, and the Test will be finished.

## Options screen

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/opciones.png "Options")

This is the screen where you can select:
* Whether you want the program to check for file association.
* Whether you want the splash screen to show at startup.
* Whether you want to see the pattern editor with an included form, or a standard one.
* The User Interface language.

## Frequencies table editor

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/tablaFreq.png "Frequencies table editor")

This is the editor where notes frecuencies can be customized. You can select:
* A "standard" table for MSX/Spectrum
* A "standard" table for CPC
* A parameterized table: Change the slider value to make the frecuencies higher or lower.
* A custom table, where you can specify the values for each note in every octave.
* You can save the table and load it for use in different songs.

## Arpeggio editor
This editor will let you easily specify one channel of a pattern, with some composer visual aids.

![alt text](https://raw.githubusercontent.com/AugustoRuiz/WYZTracker/master/docs/imgs/arpegios.png "Arpeggio editor")

You can specify a set of parameters that control the root note and how many octaves you want to have available to create the arpeggio, and the target pattern and channel where the notes will be set.

The parameters are:
* Instrument: The instrument to use for the arpeggio
* Initial octave: Root note octave.
* Initial note: Root note.
* Length: Number of notes of the arpeggio.
* Highlight by: This will allow you to see in the lower part the notes of the specified scale highlighted.
* Target pattern: The pattern where the notes will be set when you click "Apply". It can be set to "New pattern", and a new pattern will be created, and added to the pattern order list.
* Pattern length: Only available when the target pattern is a new pattern, it sets the length of the new pattern.
* Channel: Target channel the notes will be set to.
* Max. Octave: Number of octaves shown around the root note.

On the lower part of the editor you can see a grid where you can set notes for the arpeggio. The notes are set by clicking with the left mouse button (and are shown in red). Clicking with the right mouse button, you will delete the note if there is a note in the current column, or insert a "stop playing" note (shown in orange).

The buttons available are:

* Apply: Copies the notes specified in the target pattern/channel.
* Load: Loads a previously saved arpeggio configuration.
* Save: Saves the current arpeggio configuration.
* Randomize: Creates a random set of notes in the current arpeggio, deleting the current notes! If any scale is selected, only highlighted notes will be used for the random notes.
* Test: Plays the current arpeggio.
* Lock: As in the instrument editor and FX editor, locking will allow you to avoid changing column when dragging with the mouse.

## <a name="keyreference"></a>Keys reference:

`Return` -> Play/Stop  
`Escape` -> Stop  

`F2` -> Decrease selected octave (the one to be used in next notes, not the selected note)  
`F3` -> Increase selected octave  
`F4` -> Decrease edition increment  
`F5` -> Increase edition increment  
`F6` -> Select previous instrument  
`F7` -> Select next instrument  
`F8` -> Clear instrument  
  
`Cursors/PageUp/PageDown/Home/End` -> Move selection  
`Backspace/Delete` -> Erase selection  
  
`Shift + Cursors/PageUp/PageDown` -> Modify selection  
`Shift + [0-9A-Z]` -> Set Volume to selection (sets volume modifiers so that initial volume in note - first volume in instrument - is the specified value)  
`Shift + Space` -> Remove volume modifier  
  
`Alt + Cursor Up` -> Set next FX / note (depending on selection)  
`Alt + Cursor Down` -> Set previous FX / note (depending on selection)  
`Alt + Cursor Left` -> Set previous octave  
`Alt + Cursor Right` -> Set next octave  
  
`Ctrl + Cursor Up` -> Increase Volume modifier in selection.  
`Ctrl + Cursor Down` -> Decrease Volume modifier in selection.  
`Ctrl + PgDn` -> Next Pattern  
`Ctrl + PgUp` -> Previous Pattern  
`Ctrl + Shift + V` -> Paste as Delay (pastes copied notes with the specified delay volume decrement already applied).  
`Ctrl + Shift + Cursor Up` -> Increase tempo to selection  
`Ctrl + Shift + Cursor Down` -> Decrease tempo to selection  
`Ctrl + Alt + [0-9A-Z]` -> Select instrument / fx (depending on selection). Applies the instrument/fx to the selection.  
`Ctrl + Alt + R` -> Set sawtooth to selection, select sawtooth (if selection in channels other than FX)  
  
For the FX editor:  
  
`Left/Right`: Move selection.  
`Cursor Up/Down`: Modify volume to selection.  
`Shift + Cursor Up/Down/PgUp/PgDown`: Modify envelope frequency sliders to selection.  
`Shift + Cursor Left/Right`: Change envelope to selection  
`Alt + Cursor Up/Down`: Modify noise sliders to selection  
`Ctrl + Cursor Up/Down/PgUp/PgDown`: Modify frequency sliders to selection.  
`Virtual Piano Keys`: If Test check is pressed, test the FX. Otherwise, set frequency to that of the pressed note.  
  
### Keyboard reference (Virtual Piano)

Note | Lower octave | Lower octave # | Higher octave | Higher octave #
-----|--------------|----------------|---------------|-----------------
C  | Z |   | Q |
C# |   | S |   | 2
D  | X |   | W |
D# |   | D |   | 3 
E  | C |   | E |
F  | V |   | R
F# |   | G |   | 5
G  | B |   | T |
G# |   | H |   | 6 
A  | N |   | Y |
A# |   | J |   | 7
B  | M |   | U |
