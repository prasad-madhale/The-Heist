# The Heist

##### Overview
“The Heist” is an AI adaption of a game “Stolen in 60 seconds”. It’s a burglary game where the AI needs to carefully plan and execute each robbery.
The Agent has to carry out a successful robbery and escape within the given time limit.

![MenuScreen](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/MenuScreen.PNG)
##### Instructions:
The project consists of two modes of operation first being the already trained models and, another the training phase which has to be accessed by the user by opening the project in Unity.

_Instructions to run the pre-trained models in Unity_:

1. Clone the project
2. Open Unity, choose open a project option and open the cloned project
3. Open ‘ChooseLevel’ scene from the Scenes folder in the assets and hit ‘Play’
4. Once in the scene select the map(Easy/Hard) and agent persona(Tactical/Naive/Smart/Safe) you want to run
5. Once in the game you can exit to the ChooseLevel scene at anytime using the ‘Esc’ key

_Instructions to train the models in Unity:_

1. Follow steps 1 and 2 as mentioned above
2. Open the ‘EasyLevel’ or ‘HardLevel’ scene depending upon your choice of the
map and select the ‘agent’ object from the hierarchy that appears to the left
3. Under the Agent’s inspector (which is to the right) there is a field called agent_type select the type of agent to train using this field: Set agent type to 1,2,3,4 for Tactical agent, Smart agent, Naive agent, Safe agent respectively.
4. Then find the Training field which is situated below the agent_type field. Set the training field to True. (Very important step)
5. Hit play to observe the agent training in the selected map

_Note_: the trained model for the agent is stored as a text file in models directory and the agents performance measure is stored in the 'training_info' folder

##### Important points to remember:
1. The agents are only trained on the EasyLevel
2. When agents are run on the Hardlevels they use only the data that they gathered on the easy levels
3. Every training overwrites the old trained model
4. The agents find an optimal path pretty quickly on the easy map (in around 15 to 25 trails) and over-training the agent leads to unexpected behavior sometimes

#### Agent
The agent performs the actions and can be trained using different algorithms like Q-learning or SARSA or it can use the decision tree algorithm.

The agent has 4 different personalities which are as follows:

#### _Tactical agent_
 This model works on the decision tree move finder algorithm. It efficiently completes both Easy and Hard levels without prior level knowledge as it is basically a hard-coded agent.
 
 _Tactical Agent working on Easy Level_
 ![Tactical Agent working on Easy Level](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/EasyTactical.gif)

_Tactical Agent working on Hard Level_
![Tactical Agent working on Hard Level](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/HardTactical.gif)

**Expected Behavior**:

* Visit all the treasures once
* Go to exit when the Agent’s capacity is full
* Avoid Camera’s at all cost
* Pick up new loot whenever possible
* Exchange Loot: Give away less valuable loot for a more valuable loot
* Flee when all the loot is collected
* Flee when time remaining is less

**Trained On**: Doesn’t require training

**Tested On**: Easy and Hard Levels

**Observed Behavior on Easy Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Avoided Camera’s at all cost
* Picked up new loot whenever possible
* Exchanged Loot: Gave away less valuable loot for a more valuable loot
* Flee when all the loot was collected

**Observed Behavior on Hard Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Avoided Camera’s at all cost
* Picked up new loot whenever possible
* Flee when all the loot was collected

#### _Naive agent_
Uses Q-Learning and is a Naive agent.This model is short-sighted as it has a small gamma value of 0.1. It’s the most innocent agent who is looting for the first time therefore doesn’t exchange less value goods for higher valued ones.

_Naive agent working on Easy Level_
![Naive agent working on Easy Level](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/EasyNaive.gif)

**Expected Behavior**:

* Visit all the treasures once
* Go to exit when the Agent’s capacity is full
* Pick up new loot whenever possible
* Flee when all the loot is collected
* Flee when time remaining is less

**Trained On**: Trained on easy level

**Tested On**: Easy and Hard Levels

**Observed Behavior on Easy Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Went in front of camera to receive 10 second penalty
* Picked up new loot whenever possible
* Flee when all the loot was collected

**Observed Behavior on Hard Level**:

* Visited all the treasures once
* Looted all but two treasures
* Went to exit when the Agent’s capacity was full
* Went in front of camera to receive 10 second penalty
* Picked up new loot whenever possible
* Could not flee before the timer was up

#### _Smart agent_
Uses Q-Learning and is the smartest agent. This agent has a slight touch of cunningness because it always exchanges loot for a better loot when it doesn’t have capacity for the new loot. But it doesn't avoid cameras.

_Smart agent working on Easy Level_
![Smart agent working on Easy Level](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/EasySmart.gif)

**Expected Behavior**:

* Visit all the treasures once
* Go to exit when the Agent’s capacity is full
* Pick up new loot whenever possible
* Exchange Loot: Give away less valuable loot for a more valuable loot
* Flee when all the loot is collected
* Flee when time remaining is less

**Trained On**: Trained on easy level

**Tested On**: Easy and Hard Levels

**Observed Behavior on Easy Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Went in front of camera to receive 10 second penalty
* Picked up new loot whenever possible
* Exchanged Loot: Gave away less valuable loot for a more valuable loot
* Flee when all the loot was collected

**Observed Behavior on Hard Level**:

* Visited all the treasures once
* Looted all but two treasures
* Went to exit when the Agent’s capacity was full
* Went in front of camera to receive 10 second penalty
* Picked up new loot whenever possible
* Could not flee before the timer was up

#### _Safe agent_
Uses SARSA algo and is the safest agent. It doesn't take risk of getting detected. It even goes to the exit when the weight crosses the threshold of 65%. It always fears cameras while looting and hence will never go in front of one. It exchanges less value goods for higher valued ones.

_Safe agent working on Easy level_
![Safe agent working on Easy level](https://github.com/prasadchelsea33/The-Heist/blob/master/GIFS/EasySafe.gif)

**Expected Behavior**:

* Visit all the treasures once
* Go to exit when the Agent’s capacity is full
* Avoid Camera’s at all cost
* Pick up new loot whenever possible
* Exchange Loot: Give away less valuable loot for a more valuable loot
* Flee when all the loot is collected
* Flee when time remaining is less

**Trained On**: Trained on easy level

**Tested On**: Easy and Hard Levels

**Observed Behavior on Easy Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Avoided Camera’s at all cost
* Picked up new loot whenever possible
* Exchanged Loot: Gave away less valuable loot for a more valuable loot
* Flee when all the loot was collected

**Observed Behavior on Hard Level**:

* Visited all the treasures once
* Looted all the treasures
* Went to exit when the Agent’s capacity was full
* Avoided Camera’s at all cost
* Picked up new loot whenever possible
* Could not flee before the timer was up

#### A* for pathfinding:
The agent uses A* algorithm for pathfinding. A* uses a heuristic function in order to figure out the path, for this game we use Euclidean distance as the heuristic function for the A* algorithm.


#### Q-Learning / SARSA details:

**Possible Moves**: 6

**Possible States**: 512

**Total Q-Points**: 3072

#####MOVES:

We have defined 6 moves for any given agent:

1. _**GO TO TARGET**_ -
This move will take the player to next available treasure and will also make the
player to break it.

2. _**GO TO EXIT**_ -
This move will take the player to exit.

3. _**PICK UP TREASURE**_ -
This move has a dual functionality. For the Naïve Agent, only simple pick-up
operation will be performed. But, the rest all advance agents use pick-up with an
exchange-loot policy.

4. _**DROP TREASURE AT EXIT**_ -
This move will enable player to drop the treasure at the exit when she/he is near
the exit.

5. _**FLEE**_ -
This move will take the player to exit, make it drop the looted treasure at exits, and this move will also end the game.

6. _**SKIP TARGET**_ -
This move will skip the next considered target due to reasons when set. The
Tactical and Safe Agents usually try to avoid cameras in order to get more time.

#### States in Q-Learning and SARSA:

Any agent will be in a combination of following 9 states:

1. _**WEIGHT STATE**_ -
This state returns 1 if the capacity of the Heist is full, otherwise it will return zero.
Safe Agent (SARSA) been a safety-first agent, sets this sub-state to 1 after its
reached 65% of its capacity. Agent is expected to go to the exit and drop weights
in this case.

2. _**FLEE STATE**_ -
This state is set to 1 when the count-down has struck below the time agent would
require reaching the exit, otherwise its zero. This value is checked for every
frame. Agent is expected to ignore other tasks and reach to the exit.

3. _**ALL TARGETS VISITED STATE**_ -
This state is set to 1 when the agents visits and loots (or tries to loot) all the treasure, otherwise its zero. Agent is expected to Flee to the exit after that.

4. _**TIME STATE**_ -
This state is set to 1 when the agents does have enough time to travel to the next treasure, break it open and then travel back to exit, otherwise its zero. Agent is expected to go to next treasure in this case.

5. _**DISTANCE TO TARGET STATE**_ -
This state is set to 1 distance to exit is greater than distance to the target,
otherwise its zero. Agent is expected to go to next treasure in this case.

6. _**DETECTION STATE**_ -
This state is set to 1 when the next treasure (target) selected is secured,
otherwise its zero.

7. _**RISK STATE**_ -
This state is set to 1 when at least 80% of the time has elapsed during the game,
otherwise its zero.

8. _**AT OPEN TREASURE STATE**_ -
This state is set to 1 when at Agent is near an opened treasure, otherwise its
zero.

9. _**AT EXIT STATE**_ -
This state is set to 1 when at Agent is near an exit, otherwise its zero.
