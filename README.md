![image](https://github.com/user-attachments/assets/82ff9682-1767-4a75-b878-a2ed0d210e75)

Given this starting point for autopilot and AI features, I strongly feel we should have an "Intent-classification" of user queries. Indeed, user can literally type in anything over here, and we should be able to handle it (the alternative being showing error every time, which is not good).

I have developed a POC that classifies the intent of input query into:

1. search on artifacts
2. search not on artifacts
3. action on artifacts
4. action not on artifacts
5. others

Take a look at the CSV file for clarity (1L examples randomly generated and extended using PnC)

Change the input in the program.cs file and run. An example output is as follows:

![image](https://github.com/user-attachments/assets/7c00d78b-ec86-4044-aea5-22d65f5c7cbf)
![image](https://github.com/user-attachments/assets/b3a47b55-c2d8-42fe-8bea-c94e21717f99)

NB: 
1. This is by no means a perfect implementation. However, it does not prompt GPT (in-house solution) and can defiintely be made better. 
2. This feature in itself however can be v v beneficial. We can extend "search" limitlessly, and develop BE code in a modular way according to this "intent" (many applications and possible paths).
