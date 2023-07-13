using System.ComponentModel;

public enum Mission
{
	None = 0,
	[Description("Give Jerome the yo-yo during morning time.")]
	JeromeGiveYoyo = 1,
	[Description("Leave the classroom while the teacher is distracted.")]
	JeromeLeaveClassroom = 2,
	[Description("Get into the janitor's closet.")]
	JeromeGoToCloset = 3,
	[Description("Get the laser pointer out of the 'stolen stuff' box.")]
	JeromeGetLaserFromBox = 4,
	[Description("Bring the laser pointer back to Jerome.")]
	JeromeBringLaser = 5,
	[Description("Hide the laser pointer.")]
	JeromeHideLaser = 6,
	[Description("Get the laser pointer from the bathroom before lunch ends.")]
	JeromeGetLaserFromBathroom = 7,
	[Description("Try to bring the laser pointer to Jerome...again.")]
	JeromeBringLaserRecess = 8,
	[Description("Save Jerome from the Janitor before recess ends.")]
	JeromeSaveFromJanitor = 9,
	[Description("Place the device behind the janitor.")]
	JeromePlaceDevice = 10,
	[Description("Tell Monty you placed the device.")]
	JeromeTellMontyDevicePlaced = 11,
	[Description("Solve the duck riddle.")]
	JeromeSolveRiddle = 12,
	[Description("Find out when the teacher is alone.")]
	BuggsFindOutWhenAlone = 13,
	[Description("Tell Buggs when the teacher is alone.")]
	BuggsTellWhenAlone = 14,
	[Description("Talk to Buggs during lunch.")]
	BuggsTalkAtLunch = 15,
	[Description("Talk to the other kids to see if they have any dirt on them.")]
	BuggsFindOutGoldStars = 16,
	[Description("See if Monty has a distraction of some kind.")]
	BuggsCheckForDistraction = 17,
	[Description("Tell Buggs you got a distraction device.")]
	BuggsTellGotDistraction = 18,
	[Description("Place the distraction device under one of the front tables.")]
	BuggsPlaceDevice = 19,
	[Description("Use the lunch pass to have lunch with the teacher.")]
	BuggsGoToLunchWithTeacher = 20,
	[Description("Kill the teacher.")]
	BuggsKillTeacher = 21,
	[Description("Talk to Buggs at recess to find out what to do with the knife.")]
	BuggsDisposeOfKnife = 22,
	[Description("Convince Nugget to let you hide the knife in the Nugget Cave.")]
	BuggsConvinceHide = 23,
	[Description("Put the gum Cindy gave you in Lily's hair during morning time. lol")]
	CindyPutGumInHair = 24,
	[Description("Tell Cindy you put the gum in Lily's hair.")]
	CindyTalkMorningTime = 25,
	[Description("Go to the bathroom to clean yourself up.")]
	CindyWashUp = 26,
	[Description("Play house with Cindy.")]
	CindyPlayHouse = 27,
	[Description("Have lunch with Cindy.")]
	CindyEatLunch = 28,
	[Description("Find something 'vegan' for Cindy to eat.")]
	CindyFindVegan = 29,
	[Description("Go to the janitor's closet.")]
	CindyGoToCloset = 30,
	[Description("Find something gross in the janitor's closet.")]
	CindyFindSomethingGross = 31,
	[Description("Head back to the cafeteria.")]
	CindyGoBackToCafeteria = 32,
	[Description("Show Cindy the bucket of blood.")]
	CindyShowBucket = 33,
	[Description("Dump the bucket on Lily.")]
	CindyDumpBucket = 34,
	[Description("Show Cindy the piece of paper.")]
	CindyShowPaper = 35,
	[Description("Find someone who can read the paper.")]
	CindyReadPaper = 36,
	[Description("Tell Cindy about the recipe.")]
	CindyTellPaper = 37,
	[Description("Tell Cindy about the recipe.")]
	CindyTellAboutDog = 38,
	[Description("Put the gum in Lily's hair during lunch.")]
	CindyPutGumLunch = 39,
	[Description("Tell Cindy you put the gum in Lily's hair.")]
	CindyTellPutGumLunch = 40,
	[Description("Show Lily the note Nugget gave you before morning time.")]
	LilyShowNote = 41,
	[Description("Find someone to read the note.")]
	LilyReadNote = 42,
	[Description("Tell Lily that Monty is deciphering the note before morning time starts.")]
	LilyTellNote = 43,
	[Description("Get the deciphered note from Monty at lunch.")]
	LilyGetNoteFromMonty = 44,
	[Description("Show the deciphered note to Lily during lunch.")]
	LilyShowCode = 45,
	[Description("Get sent to the principal's office alone.")]
	LilyGetSentToOffice = 46,
	[Description("Find the hatch the note mentioned in the principal's office.")]
	LilyLookForSomethingSuspicious = 47,
	[Description("Tell Lily about the hatch during morning time.")]
	LilyTellAboutTheHatch = 48,
	[Description("Figure out how to lure the principal out of his office.")]
	LilyFindWayToLure = 49,
	[Description("Figure out how to get a key to the principal's office.")]
	LilyFindWayToGetKey = 50,
	[Description("Talk to Lily during lunch.")]
	LilyTalkAtLunch = 51,
	[Description("Meet Lily in the bathroom.")]
	LilyMeetInBathroom = 52,
	[Description("Call the principal from the bathroom.")]
	LilyCallInThreat = 53,
	[Description("Use the key to get into the principal's office.")]
	LilyUseKey = 54,
	[Description("Find a way to open the hatch.")]
	LilyOpenHatch = 55,
	[Description("Find evidence of Billy's disappearance in the bathroom.")]
	LilyFindEvidence = 56,
	[Description("Show Lily the evidence of Billy's disapperance.")]
	LilyShowEvidence = 57,
	[Description("Save Billy.")]
	LilySaveBilly = 58,
	[Description("Collect the five nuggets of friendship.")]
	NuggetCollectNuggets = 59,
	[Description("Talk to Nugget during morning time.")]
	NuggetStartCollectNuggets = 60,
	[Description("Get the nugget of friendship from Nugget's cubby.")]
	NuggetGetCubbyNugget = 61,
	[Description("Ask Nugget how to get the next nugget of friendship.")]
	NuggetStartThirdNugget = 62,
	[Description("Give Nugget's love letter to Lily.")]
	NuggetGiveLoveLetter = 63,
	[Description("Tell Nugget you delivered the letter.")]
	NuggetDeliveredLetter = 64,
	[Description("Talk to Nugget about getting the next nugget at lunch.")]
	NuggetTalkAtLunch = 65,
	[Description("Kill Buggs during lunch.")]
	NuggetKillBuggs = 66,
	[Description("Get Buggs to eat the poisoned nugget.")]
	NuggetPoisonBuggs = 67,
	[Description("Get the antidote from Nugget.")]
	NuggetGetAntidote = 68,
	[Description("Tell Nugget you stabbed Buggs.")]
	NuggetTellStabbedBuggs = 69,
	[Description("Tell Nugget you poisoned Buggs.")]
	NuggetTellPoisonedBuggs = 70,
	[Description("Talk to Nugget about the last nugget of friendship at recess.")]
	NuggetTalkAtRecess = 71,
	[Description("Place the device on the statue.")]
	NuggetPlaceDevice = 72,
	[Description("Talk to Lily about Billy's disappearance to trigger the device.")]
	NuggetTalkToLily = 73,
	[Description("Collect the last nugget of friendship to enter the Nugget Cave.")]
	NuggetGetLastNugget = 74,
	[Description("Talk to Nugget during lunch.")]
	NuggetJoinAtLunch = 75,
	[Description("Get Nugget a magnifying glass.")]
	NuggetGetMagnifyingGlass = 76,
	[Description("Ask Monty to make a key out of the mold Jerome gave you.")]
	MontyAskFormold = 77,
	[Description("Collect $20 to buy the key from Monty at the end of the day.")]
	MontyCollectTwenty = 78,
	[Description("Tell the Janitor he mispelt 'biscuit'.")]
	MontyCorrectSpelling = 79,
	[Description("Tell the teacher that Buggs threatened you.")]
	TeacherTellOnBuggs = 80,
	[Description("Start a fight with Buggs.")]
	TeacherStartFightWithBuggs = 81,
	[Description("Talk to the teacher about Jerome during morning time.")]
	TeacherTalkMorningTimeJerome = 82,
	[Description("Try to get the stolen hall pass from Jerome.")]
	TeacherTryToGetPass = 83,
	[Description("Find something to give to Jerome.")]
	TeacherFindJeromeGift = 84,
	[Description("Give the pass to the teacher.")]
	TeacherGivePass = 85,
	[Description("Gain the trust of Nugget before recess starts.")]
	TeacherNuggetGainTrust = 86,
	[Description("Tell the teacher you gained Nugget's trust.")]
	TeacherTellGainedTrust = 87,
	[Description("Enter the Nugget Cave.")]
	TeacherEnterNuggetCave = 88,
	[Description("Collect evidence of Nugget's wrong doing.")]
	TeacherCollectEvidence = 89,
	[Description("Show the teacher the dog remains.")]
	TeacherShowEvidence = 90,
	[Description("Get Lily in trouble.")]
	TeacherGetLilyInTrouble = 91,
	[Description("Tell the teacher what happened to Lily.")]
	TeacherCollectLilyStar = 92,
	[Description("Get Monty in trouble.")]
	TeacherGetMontyInTrouble = 93,
	[Description("Tell the teacher what happened to Monty.")]
	TeacherCollectMontyStar = 94,
	[Description("Get Buggs in trouble again.")]
	TeacherGetBuggsInTrouble = 95,
	[Description("Tell the teacher you stabbed Buggs.")]
	TeacherTellStabbedBuggs = 96,
	[Description("Tell the teacher you poisoned Buggs.")]
	TeacherTellPoisonedBuggs = 97,
	End = 98
}
