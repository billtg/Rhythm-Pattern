# Rhythm-Pattern
Rhythm Based circle Game

Created using Unity

Main class for determining song position is the Conductor class in the SpinGame folder. Next levels down are:
SequenceController
RingParent
CircleScript

Input is managed through InputManager and EventManager

Song loading is done using the SongLoader object, which holds a list of SongObjects, which each hold the information for each song
Each song is dynamically loaded at the start of opening the Main scene

UI is a little scattered, but is all in the UI folder

Feel free to play around with it or build something off of it!
