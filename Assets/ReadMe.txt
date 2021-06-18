This folder simply contains assets that were using during the developmentof this engine.
During development these assets were accessed from hard coded absolute paths. Obviously this was intented to be changed at some point
and the places that use such paths in the engine itself support (always?) passing in filepaths for assets as an argument.

Anyway the testing scripts won't work without these assets so to make seeing the demos easy I have places the assests here and updated the code
to use relative paths to this folder where the are referenced. This should allow you to simply click Start in visual studio and see whichever test is set
to run in TestingProject.Program.cs