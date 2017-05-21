
		org	$4000, $bfff
ROM_START:	db	"AB"		; ID ("AB")
		dw	SPOINT		; INIT
		ds	$4010 - $, $00	; STATEMENT, DEVICE, TEXT, Reserved


; MSX BIOS

ENASLT:		EQU	$0024 ; Enable any slot permanently
RSLREG:		EQU	$0138 ; Read Primary Slot Register

; VARIABLES DEL SISTEMA

CLIKSW:		EQU	$F3DB ; Keyboard click sound
EXPTBL:		EQU	$FCC1 ; Set to $80 during power-up if Primary Slot is expanded (4b)

;CONTROL DE LAS INTERRUPCIONES

HOOK:		EQU	$FD9A


; AJUSTES INICIALES

SPOINT:
; Reads the primary slot of the page 1
		call    RSLREG	; a = 33221100
		rrca
		rrca
		and	$03	; a = xxxxxxPP
		ld	c, a	; c = xxxxxxPP
; Reads the expanded slot flag
		ld	hl, EXPTBL ; EXPTBL + a => EXPTBL for slot of page 1
		add	a, l
		ld	l, a
		ld	a, [hl]
		and	$80	; a = Exxxxxxx
; Defines slot ID (1/2)
		or	c	; a = ExxxxxPP
		ld	c, a	; c = ExxxxxPP
; Reads the secondary slot selection register
		inc	l ; hl += 4 => = SLTTBL for slot of page 1
		inc	l
		inc	l
		inc	l
		ld	a, [hl]
		and	$0c	; a = xxxxSSxx
; Define slot ID (2/2)
		or	c	; a = ExxxSSPP
; Enable
		ld	h, $80 ; Bit 6 and 7: page 2 ($8000)
		call	ENASLT






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

INCLUDE	"EXTERNAL.MUS.asm"

SONG_0:		INCBIN	"EXTERNAL.MUS" ;

TABLA_SONG:	DW	SONG_0;SONG_1

;código del player

INCLUDE	"WYZProplay47cMSX.asm"

;variables del player

		org	$e000, $f380		;8KB de RAM
		
INCLUDE	"WYZProplay47c_RAM.tniasm.asm"
