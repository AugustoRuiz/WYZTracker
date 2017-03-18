
; Tabla de instrumentos
TABLA_PAUTAS: DW PAUTA_0,PAUTA_1,PAUTA_2,PAUTA_3,PAUTA_4,PAUTA_5,PAUTA_6,PAUTA_7,PAUTA_8,PAUTA_9,PAUTA_10,PAUTA_11

; Tabla de efectos
TABLA_SONIDOS: DW SONIDO0,SONIDO1,SONIDO2,SONIDO3,SONIDO4,SONIDO5,SONIDO6,SONIDO7

;Pautas (instrumentos)
;Instrumento '07-'
PAUTA_0:	DB	10,-1,10,1,10,-1,10,1,10,0,10,0,10,0,6,0,6,0,130
;Instrumento '22-'
PAUTA_1:	DB	10,-1,9,4,8,-3,6,1,6,-1,130
;Instrumento '23-'
PAUTA_2:	DB	6,1,6,0,129
;Instrumento '10-037'
PAUTA_3:	DB	8,0,24,3,24,7,131
;Instrumento '10-047'
PAUTA_4:	DB	8,0,24,4,24,7,131
;Instrumento 'Bass-'
PAUTA_5:	DB	43,0,11,-16,11,16,11,-16,10,16,9,-8,9,8,9,8,9,-8,8,0,8,0,129
;Instrumento 'Bass cont'
PAUTA_6:	DB	40,-2,8,2,8,2,8,-2,8,-2,132
;Instrumento 'OCTAVE+1'
PAUTA_7:	DB	8,0,24,1,24,2,24,3,24,4,24,5,24,6,24,7,24,8,24,9,24,10,24,11,24,12,129
;Instrumento 'Med 07- vibr'
PAUTA_8:	DB	8,-2,8,1,8,0,8,1,8,0,8,1,8,0,8,0,8,0,8,-1,8,0,8,-1,8,0,8,-1,8,0,8,0,8,0,144
;Instrumento 'Med 07-'
PAUTA_9:	DB	8,-1,8,0,8,0,8,0,8,0,8,0,131
;Instrumento 'slide up'
PAUTA_10:	DB	8,-2,129
;Instrumento 'Flauta'
PAUTA_11:	DB	6,-3,8,4,9,-4,9,4,8,-1,8,0,8,1,8,0,132

;Efectos
;Efecto 'bass drum'
SONIDO0:	DB	0,80,128,131,1,9,0,112,128,0,4,1,255
;Efecto 'drum'
SONIDO1:	DB	127,32,128,0,2,9,0,0,136,0,3,1,0,0,135,0,4,1,255
;Efecto 'hithat'
SONIDO2:	DB	0,7,1,255
;Efecto 'bass drum vol 2'
SONIDO3:	DB	0,90,0,0,101,0,255
;Efecto 'Hit hat 2'
SONIDO4:	DB	0,6,16,255
;Efecto 'Bongo 1'
SONIDO5:	DB	186,16,128,0,2,9,232,16,128,0,3,1,0,32,128,0,4,1,69,32,128,0,5,1,255
;Efecto 'Bongo 2'
SONIDO6:	DB	69,32,128,0,2,9,186,32,128,0,3,1,46,48,128,0,4,1,232,48,128,0,5,1,255
;Efecto 'Nuevo'
SONIDO7:	DB	0,10,16,0,10,16,0,10,16,255

;Frecuencias para las notas
DATOS_NOTAS: DW 0,0
DW 958,904,853,805,760,717,678,639,604,569
DW 538,507,479,452,427,403,380,359,339,320
DW 302,285,269,254,240,226,213,202,190,179
DW 169,160,151,142,134,127,120,113,106,101
DW 95,90,85,80,76,71,67,63,60,57
DW 53,50,48,45,43,40,38,36,34,32
