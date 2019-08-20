# slack-wiki-game

_Note: Very much a work-in-progress; A proof-of-concept currently lives on master, however the proof of concept only supports one game at a time and has a few settings that are hard-coded that really should be in a configuration file_


## Notes for building
- Set the enviorment variable "WIKI_BOT_USER_OATH_TOKEN" to the bot user OATH token before building
- Modify SecretsManager 
## Play Instructions
1. Start a challenge by posting two links into the slack channel.

e.g
  >Today's Challenge:
https://en.wikipedia.org/wiki/Buddy_Guy -> https://en.wikipedia.org/wiki/Ramen

2. Reply to the thread with your solution in the format "Start Page Name -> Step 1 Page Name -> Step 2 Page Name -> ... -> End Page Name"

e.g:
  > Buddy Guy -> Chicago -> China -> Japan -> Japanese Noodles -> Ramen
  
## TODOs
1. Database Integration
2. Aggregate Stastics
3. Automated Game Completion
4. Path Confirmation
