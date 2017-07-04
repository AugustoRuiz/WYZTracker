
		.filename "EXTERNAL"
		.page	1
		.size	32			;ROM de 32KB
		.ROM


; VARIABLES DEL SISTEMA

CLIKSW		EQU	$F3DB

;CONTROL DE LAS INTERRUPCIONES

HOOK		EQU	$FD9A


; AJUSTES INICIALES

SPOINT:		.search				;ROM de 32KB





AJUSTES:	DI
		CALL	PLAYER_INIT



; MUSICA DATOS INICIALES

		LD	A,0			;REPRODUCIR LA CANCION Nº 0
		CALL	CARGA_CANCION





;INICIA INTERRUPCIONES

		LD	HL,INICIO
		LD	[HOOK+1],HL
		LD	A,$C3
		LD	[HOOK],A
		EI
		LD	A,$0E
		LD	[PSG_REG+13],A

LOOP:		JP	LOOP

;archivo de instrumentos

.INCLUDE	"EXTERNAL.MUS.asm"

SONG_0:		.INCBIN	"EXTERNAL.MUS" ;

TABLA_SONG:	DW	SONG_0;SONG_1

;código del player

.INCLUDE	"WYZProplay47cMSX.asm"

;variables del player

		.org	$E000			;8KB de RAM
		
.INCLUDE	"WYZProplay47c_RAM.asm"
