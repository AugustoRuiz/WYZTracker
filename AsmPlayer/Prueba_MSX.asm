
                .filename "EXTERNAL"
                .page	1
		.ROM


; VARIABLES DEL SISTEMA

CLIKSW      	EQU	$F3DB


; AJUSTES INICIALES

SPOINT:		.search





AJUSTES:	DI
		CALL	PLAYER_OFF



; MUSICA DATOS INICIALES

                LD      HL,$DE00        	;* RESERVAR MEMORIA PARA BUFFER DE SONIDO!!!!!
                LD      [CANAL_A],HL		;RECOMENDABLE $10 O MAS BYTES POR CANAL.

                LD      HL,$DE20
                LD      [CANAL_B],HL

                LD      HL,$DE40
                LD      [CANAL_C],HL

                LD      HL,$DE60
                LD      [CANAL_P],HL




                LD      A,0             	;REPRODUCIR LA CANCION Nº 0
                CALL    CARGA_CANCION





;INICIA INTERRUPCIONES

                LD      HL,INICIO
                LD      [HOOK+1],HL
                LD      A,$C3
                LD      [HOOK],A
                EI
		LD	A,$0E
		LD      [PSG_REG+13],A

LOOP:		JP	LOOP

;archivo de instrumentos

.INCLUDE	"EXTERNAL.MUS.asm"

SONG_0:		.INCBIN	"EXTERNAL.MUS" ;

TABLA_SONG:     DW      SONG_0;SONG_1

;código del player

.INCLUDE	"WYZPROPLAY47cMSX.ASM"


