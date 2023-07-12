using System.ComponentModel;

public enum MissionSpanish
{
	[Description("Entregar a Jerome el yo-yo en las clase de la mañana.")]
	JeromeGiveYoyo = 0,
	[Description("Salir del salón cuando la maestra este distraída.")]
	JeromeLeaveClassroom = 1,
	[Description("Meterse en el armario del conserje.")]
	JeromeGoToCloset = 2,
	[Description("Sacar el puntero láser de la caja de objetos robados.")]
	JeromeGetLaserFromBox = 3,
	[Description("Devolver el puntero láser  a Jerome.")]
	JeromeBringLaser = 4,
	[Description("Esconder el puntero láser.")]
	JeromeHideLaser = 5,
	[Description("Sacar el puntero laser del baño, antes de que termine el almuerzo.")]
	JeromeGetLaserFromBathroom = 6,
	[Description("Tratar de entregar el puntero láser a Jerome…de nuevo.")]
	JeromeBringLaserRecess = 7,
	[Description("Salvar a Jerome del conserje antes de que termine el recreo.")]
	JeromeSaveFromJanitor = 8,
	[Description("Colocar el dispositivo detrás del conserje.")]
	JeromePlaceDevice = 9,
	[Description("Decir a Monty que colocaste el dispositivo.")]
	JeromeTellMontyDevicePlaced = 10,
	[Description("Resolver el acertijo del pato.")]
	JeromeSolveRiddle = 11,
	[Description("Averiguar cuando la maestra está sola.")]
	BuggsFindOutWhenAlone = 12,
	[Description("Avisar a Buggs cuando la maestra este sola.")]
	BuggsTellWhenAlone = 13,
	[Description("Hablar con Buggs en el almuerzo.")]
	BuggsTalkAtLunch = 14,
	[Description("Hablar con otros niños para averiguar si están metidos en el chisme.")]
	BuggsFindOutGoldStars = 15,
	[Description("Ver si Monty tiene algún tipo de distracción .")]
	BuggsCheckForDistraction = 16,
	[Description("Decir a Buggs que tienes un dispositivo de distracción.")]
	BuggsTellGotDistraction = 17,
	[Description("Colocar el dispositivo de distracción debajo de una de las mesas delanteras.")]
	BuggsPlaceDevice = 18,
	[Description("Usar el pase de almuerzo para almorzar con la maestra.")]
	BuggsGoToLunchWithTeacher = 19,
	[Description("Matar a la maestra.")]
	BuggsKillTeacher = 20,
	[Description("Hablar con Buggs en el recreo para decidir que hacer con el cuchillo.")]
	BuggsDisposeOfKnife = 21,
	[Description("Convencer a Nugget  que te deje ocultar el cuchillo en la Cueva de Nugget.")]
	BuggsConvinceHide = 22,
	[Description("Pegar el chicle que te dio Cindy en el cabello de Lily en la mañana. ")]
	CindyPutGumInHair = 23,
	[Description("Decir a Cindy que pegaste el chicle en el cabello de Lily.")]
	CindyTalkMorningTime = 24,
	[Description("Ir al baño para limpiarte.")]
	CindyWashUp = 25,
	[Description("Jugar a la casita con Cindy.")]
	CindyPlayHouse = 26,
	[Description("Almorzar con Cindy.")]
	CindyEatLunch = 27,
	[Description("Encontrar comida 'vegetariana' para Cindy.")]
	CindyFindVegan = 28,
	[Description("Ir al armario del conserje.")]
	CindyGoToCloset = 29,
	[Description("Buscar algo asqueroso en el armario del conserje.")]
	CindyFindSomethingGross = 30,
	[Description("Regresar a la cafetería.")]
	CindyGoBackToCafeteria = 31,
	[Description("Mostrar a Cindy el cubo de sangre.")]
	CindyShowBucket = 32,
	[Description("Arrojar el cubo sobre Lily.")]
	CindyDumpBucket = 33,
	[Description("Mostrar a Cindy el pedazo de papel.")]
	CindyShowPaper = 34,
	[Description("Encontrar a alguien quien pueda leer el papelito.")]
	CindyReadPaper = 35,
	[Description("Hablar con Cindy sobre la receta.")]
	CindyTellPaper = 36,
	[Description("Hablar con Cindy sobre el perro.")]
	CindyTellAboutDog = 37,
	[Description("Pegar el chicle en el cabello de Lily durante el almuerzo.")]
	CindyPutGumLunch = 38,
	[Description("Decir a Cindy que pegaste el chicle en el cabello de Lily.")]
	CindyTellPutGumLunch = 39,
	[Description("Mostrar a Lily la nota que Nugget te dio antes de las clases de la mañana.")]
	LilyShowNote = 40,
	[Description("Encontrar a alguien que puede leer la nota.")]
	LilyReadNote = 41,
	[Description("Decir a Lily antes de que empiece la clase de la mañana que Monty está descifrando la nota.")]
	LilyTellNote = 42,
	[Description("Llevar la nota descifrada por Monty al almuerzo.")]
	LilyGetNoteFromMonty = 43,
	[Description("Mostrar la nota descifrada a Lily en el almuerzo.")]
	LilyShowCode = 44,
	[Description("Mandar a Lily sola a la oficina del director.")]
	LilyGetSentToOffice = 45,
	[Description("Encontrar en la oficina del director, la escotilla mencionada en la nota")]
	LilyLookForSomethingSuspicious = 46,
	[Description("Contar a Lily sobre la escotilla en las clases de la mañana.")]
	LilyTellAboutTheHatch = 47,
	[Description("Encontrar la manera de sacar al director de su oficina.")]
	LilyFindWayToLure = 48,
	[Description("Encontrar la manera de obtener la llave de la oficina del director. ")]
	LilyFindWayToGetKey = 49,
	[Description("Hablar con Lily en el almuerzo.")]
	LilyTalkAtLunch = 50,
	[Description("Encontrarse con Lily en el baño.")]
	LilyMeetInBathroom = 51,
	[Description("Llamar desde el baño al director.")]
	LilyCallInThreat = 52,
	[Description("Usar la llave para entrar a la oficina del director.")]
	LilyUseKey = 53,
	[Description("Encontrar el modo de abrir la escotilla.")]
	LilyOpenHatch = 54,
	[Description("Encontrar en el baño evidencia de la desaparición de Billy.")]
	LilyFindEvidence = 55,
	[Description("Mostrar a Lily la evidencia de la desaparición de Billy.")]
	LilyShowEvidence = 56,
	[Description("Salvar a Billy.")]
	LilySaveBilly = 57,
	[Description("Recoger los cinco nuggets de la amistad.")]
	NuggetCollectNuggets = 58,
	[Description("Hablar con Nugget en la clase de la mañana.")]
	NuggetStartCollectNuggets = 59,
	[Description("Coger el nugget de la amistad del casillero de Nugget.")]
	NuggetGetCubbyNugget = 60,
	[Description("Preguntar a Nugget como encontrar el siguiente nugget de la amistad.")]
	NuggetStartThirdNugget = 61,
	[Description("Entregar a Lily la carta de amor de Nugget.")]
	NuggetGiveLoveLetter = 62,
	[Description("Decir a Nugget que entregaste la carta.")]
	NuggetDeliveredLetter = 63,
	[Description("Hablar con Nugget en el almuerzo sobre como obtener el próximo nugget.")]
	NuggetTalkAtLunch = 64,
	[Description("Matar a Buggs en el almuerzo.")]
	NuggetKillBuggs = 65,
	[Description("Hacer que Buggs se coma el nugget envenenado.")]
	NuggetPoisonBuggs = 66,
	[Description("Recibir al antídoto de Nugget.")]
	NuggetGetAntidote = 67,
	[Description("Contar a Nugget que apuñalaste a Buggs.")]
	NuggetTellStabbedBuggs = 68,
	[Description("Contar a Nugget que envenenaste a Buggs.")]
	NuggetTellPoisonedBuggs = 69,
	[Description("Hablar con Nugget en el recreo sobre el último nugget de la amistad.")]
	NuggetTalkAtRecess = 70,
	[Description("Colocar el dispositivo en la estatua.")]
	NuggetPlaceDevice = 71,
	[Description("Hablar con Lily sobre la desaparición de Billy para detonar el dispositivo.")]
	NuggetTalkToLily = 72,
	[Description("Encontrar el último nugget de la amistad para entrar en la Cueva de Nugget.")]
	NuggetGetLastNugget = 73,
	[Description("Hablar con Nugget en el almuerzo.")]
	NuggetJoinAtLunch = 74,
	[Description("Conseguir la lupa de Nugget.")]
	NuggetGetMagnifyingGlass = 75,
	[Description("Pedir a Monty que haga una llave usando el molde que Jerome te dio.")]
	MontyAskFormold = 76,
	[Description("Consigue $20 para comprar la llave a Monty en la tarde.")]
	MontyCollectTwenty = 77,
	[Description("Decirle al conserje que él deletreó mal la palabra 'galleta'.")]
	MontyCorrectSpelling = 78,
	[Description("Decir a la maestra que Buggs te amenazó.")]
	TeacherTellOnBuggs = 79,
	[Description("Pelear con Buggs.")]
	TeacherStartFightWithBuggs = 80,
	[Description("Hablar con la maestra sobre Jerome en las clases de la mañana.")]
	TeacherTalkMorningTimeJerome = 81,
	[Description("Tratar de obtener el permiso robado de Jerome.")]
	TeacherTryToGetPass = 82,
	[Description("Encontrar algo para darle a Jerome.")]
	TeacherFindJeromeGift = 83,
	[Description("Entregar el permiso a la maestra.")]
	TeacherGivePass = 84,
	[Description("Ganar la confianza de Nugget antes del  recreo.")]
	TeacherNuggetGainTrust = 85,
	[Description("Decir a la maestra que te has ganado la confianza de Nugget.")]
	TeacherTellGainedTrust = 86,
	[Description("Entrar a la Cueva de Nugget.")]
	TeacherEnterNuggetCave = 87,
	[Description("Recolectar pruebas de la mala conducta de Nuggets.")]
	TeacherCollectEvidence = 88,
	[Description("Mostrar a la maestra los restos del perro.")]
	TeacherShowEvidence = 89,
	[Description("Meter a Lily en problemas.")]
	TeacherGetLilyInTrouble = 90,
	[Description("Contarle a la maestra lo que pasó a Lily.")]
	TeacherCollectLilyStar = 91,
	[Description("Meter a Monty en problemas.")]
	TeacherGetMontyInTrouble = 92,
	[Description("Contarle a la maestra lo que pasó con Monty.")]
	TeacherCollectMontyStar = 93,
	[Description("Volver a meter en problemas a Buggs.")]
	TeacherGetBuggsInTrouble = 94,
	[Description("Contarle a la maestra  que apuñalaste a Buggs.")]
	TeacherTellStabbedBuggs = 95,
	[Description("Contarle a la maestra que envenenaste a Buggs.")]
	TeacherTellPoisonedBuggs = 96
}
