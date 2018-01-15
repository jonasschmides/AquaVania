# AquaVania
The protagonist is considered to be dead... however, he was resurrected
in an ocean temple through magic, with some sideeffects.
They have yet to learn how to be a fish - and how to stop the humans from
polluting the sea, utilizing the skills of fish and human forms alike!

## Group Members
Eduard Pranz		if17b025


Jonas Schmidbauer	ifXX_YYY

## Controls
WASD --	Basic Movement

E -- Grab / Release object


SPACE	-- Perform context-based action (e.g. Jump)
  

R -- Restart the level


ESC --	Back to menu / Quit game

## Gameplay Features
- Explore the depths as a fish, and the land as a human!
- Solve various puzzles while harnessing abilities to progress through the realm!

## Technical Features (Unity and other)
- 2D Physics
- Colliders / Triggers
- Particle systems
- Audio (SFX and BGM)

## Time Spent report
#### Where and in what areas did you spend your time. How much time did you use for feature A, B, C, how much for gameplay programming, where did you spend more/less time than anticipated?
#### How well did you manage to stay within the time budget and why?
#### Who did what in the assignment?

- EDI		2h		OST (Intro, Level 1)
- J/E		3h		Paper prototyping, inspiration, discussing story, challenges and the "world" as a collection of levels
- JONAS	1h		Setting up GIT repository, and initial project file
- EDI		1h		Testlevel, Lighting, Basic materials
- EDI		1h		Fish physics, Particles
- EDI		2h		Basic obstacles, Collectibles, Collision SFX
- EDI		3h		Triggers, Animated obstacles, Waypoints, Water currents
- EDI		1h		Fishing hooks
- EDI		0.5h	Added basic Credits and Controls screen, Report file
- EDI   2h    Working on object carry / deploy code
- EDI   3h    Transformation logic fish<-->human, based on ground/water touched.
- EDI   2h    More levels, QOL improvements
- EDI   3h    Recording more SFX, implementing audio mixer
- EDI   2h    Code Refactoring, Level "warping", Level title / subtitle / hints
- EDI	2h    Added sound mixer, another background track and a gameover SFX
- JONAS ??		Animation, Background rendering
- EDI	1h    Improved fish jumping out of water, added Jonas' sprite renderer
- EDI	2h		Added Air-mechanics (fish can't breathe outside of water), added UI to show air status
- EDI	3h		Added additional levels, reworked gamecontroller singleton


## Problems and Challenges
There was a problem trying to use objects with interfaces as components.
The editor just didn't provide the opportunity to drag objects into the script.
Solution: Stopped using interfaces - Abstract classes are the way to go!



How do parameters get carried over to another scene?
There are multiple solutions, but using a static object seemed convenient.

## Resources, Sources, References and Links
### Links to all resources, sources you are using. 

Idea to use waypoints for movement (heavily adapted) - https://answers.unity.com/questions/429623/enemy-movement-from-waypoint-to-waypoint.html
