using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class QLearning : MonoBehaviour {

    Transform target;
    NavMeshAgent agent;
    int action;
    double[,] Q = new double[STATES,POSSIBLE_MOVES];
    public static QLearning instance;
    public bool Done;

    // ACTIONS:
    public const int GO_TO_TARGET = 0;
    public const int GO_TO_EXIT = 1;
    public const int PICK_UP_ITEM = 2;
    public const int DROP_TREASURE = 3;
    public const int SKIP_TARGET = 4;
    public const int FLEE = 5;

    int runcount;
    const int POSSIBLE_MOVES = 6;
    const int STATES = 512;

    const string smart_model_path = "./models/smart_model.txt";
    const string smart_train_info = "./training_info/smart_training_info.txt";

    const string sarsa_model_path = "./models/sarsa_model.txt";
    const string sarsa_train_info = "./training_info/sarsa_training_info.txt";

    const string naive_model_path = "./models/naive_model.txt";
    const string naive_train_info = "./training_info/naive_training_info.txt";

    public const int SARSA_AGENT = 4;
    public const int NAIVE_AGENT = 3;
    public const int SMART_AGENT = 2;
    public const int TACTICAL_AGENT = 1;

    Vector3 start_pos;
    double reward = 0;
	float ittr_count = 0f;

    // HYPERPARAMETERS:
    double gamma = 0.3;
    double epsilon = 0.2;
    double alpha = 0.2;

    void Awake()
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
    }


    // Use this for initialization
    void Start() {
		if(Player.instance.agent_type == NAIVE_AGENT) {
			gamma = 0;
			epsilon = 0.9;
			alpha = 0.9;
		}
        ittr_count = 0;
		action = 0;
        runcount = 0;
        start_pos = gameObject.transform.position;
        makeEmpty();
        if(!Player.instance.training)
            readModel();
    }

    public bool busy;

    // Update is called once per frame
    void Update() {
        if (Done && Player.instance.training)
        {
            EnvReset();
			if(Player.instance.agent_type == NAIVE_AGENT) {
				ittr_count += 1f;
				float des_epsilon = (float)(Mathf.Pow ((float)0.8, ittr_count));
				epsilon = Mathf.Max ((float)0.01, des_epsilon);
				alpha = (float)(Mathf.Pow((float)0.9 , ittr_count));
			} 
        }

        if(Player.instance.visited_all_state() == 1)
        {
            Player.instance.flee = true;
        }

        if (Player.instance.agent_type == SMART_AGENT || Player.instance.agent_type == NAIVE_AGENT)
        {
            if (Player.instance.training)
            {
                // selects action using q-learning
                if (!busy)
                {
                    target = Astar.instance.targetSelector();
					if ((target != null) && target.gameObject.GetComponent<Treasure> ().secured) {
						Player.instance.detected = true;
					} else {
						Player.instance.detected = false;
					}
                    action = move_decider();
                    busy = true;
                    Tasks(action);
                }

                // executes the actions
                if ((action == GO_TO_TARGET) || (action == GO_TO_EXIT) || (action == FLEE))
                {
                    Tasks(action);
                }
            }
            else
            {
                if (!busy)
                {
                    target = Astar.instance.targetSelector();
					if ((target != null) && target.gameObject.GetComponent<Treasure> ().secured) {
						Player.instance.detected = true;
					} else {
						Player.instance.detected = false;
					}
                    // selects action based on the model
                    action = useModel();
                    Debug.Log("action" + action);
                    busy = true;
                    //executes the selected action
                    Tasks(action);
                }
                // executes the actions
                if ((action == GO_TO_TARGET) || (action == GO_TO_EXIT) || (action == FLEE))
                {
                    Tasks(action);
                }
            }
        }
        else if(Player.instance.agent_type == TACTICAL_AGENT)
        {
			if (!busy)
			{
				target = Astar.instance.targetSelector();
				if ((target != null) && target.gameObject.GetComponent<Treasure> ().secured) {
					Player.instance.detected = true;
				} else {
					Player.instance.detected = false;
				}
				// selects action based on the model
				action = tactical_moves();
				Debug.Log("action" + action);
				busy = true;
				//executes the selected action
				Tasks(action);
			}
			// executes the actions
			if ((action == GO_TO_TARGET) || (action == GO_TO_EXIT) || (action == FLEE))
			{
				Tasks(action);
			}
        }
        else if(Player.instance.agent_type == SARSA_AGENT)
        {
            if (Player.instance.training)
            {
                // selects action using q-learning
                if (!busy)
                {
                    target = Astar.instance.targetSelector();
					if ((target != null) && target.gameObject.GetComponent<Treasure> ().secured) {
						Player.instance.detected = true;
					} else {
						Player.instance.detected = false;
					}
					action = sarsa_move_decider();
                    busy = true;
                    Tasks(action);
                }

                // executes the actions
                if ((action == GO_TO_TARGET) || (action == GO_TO_EXIT) || (action == FLEE))
                {
                    Tasks(action);
                }
            }
            else
            {
                if (!busy)
                {
                    target = Astar.instance.targetSelector();
					if ((target != null) && target.gameObject.GetComponent<Treasure> ().secured) {
						Player.instance.detected = true;
					} else {
						Player.instance.detected = false;
					}
					// selects action based on the model
                    action = useModel();
                    Debug.Log("action" + action);
                    busy = true;
                    //executes the selected action
                    Tasks(action);
                }
                // executes the actions
                if ((action == GO_TO_TARGET) || (action == GO_TO_EXIT) || (action == FLEE))
                {
                    Tasks(action);
                }
            }
        }
    }


    int useModel()
    {
        int currentstate = Player.instance.get_state();
        return getAction(currentstate);
    }

    int move_decider()
    {
        action = QLearner();
        return action;
    }

    int sarsa_move_decider()
    {
        action = SarsaLearner();
        return action;
    }

  
    int QLearner()
    {
        float rnd = (float)(Random.Range(0, 101)) / 100;
        int currentstate = Player.instance.get_state();

        // random action according to epsilon-greedy 
        if (rnd < epsilon)
            action = Random.Range(0, POSSIBLE_MOVES);
        else
            action = getAction(currentstate);
         
        Debug.Log("Q-learning action: " + action);

        int nextstate = Player.instance.get_next_state(action);

		reward = rewardFunction(action);

        double currentQ = Q[currentstate, action];
        double maxQ = getMaxQ(nextstate);

        double q = (1 - alpha) * currentQ + alpha * (reward + gamma * maxQ);

        Q[currentstate, action] = q;

        // epsilon decaying
		if(Player.instance.agent_type != NAIVE_AGENT) {
			epsilon = 0.9 * epsilon;
		}
		return action;
    }

	double rewardFunction(int selected_action) {
		if (Player.instance.agent_type == NAIVE_AGENT) {
			return naive_reward_function (selected_action);
		}
		return smart_rewardFunction(selected_action);
	}


    double smart_rewardFunction(int selected_action)
    {
        if(Player.instance.flee_state() == 1)
        {
            if (selected_action == FLEE)
                return 10;
            return -10;
        }
        if (Player.instance.visited_all_state() == 1)
        {
            if (selected_action == FLEE)
            {
                return 10;
            } 
            return -5;

        }
        if (Player.instance.at_open_treasure_state() == 1)
        {
            if (selected_action == PICK_UP_ITEM)
                return 5;
            return -1;
        }
        if (Player.instance.at_exit_state() == 1)
        {
            if (selected_action == DROP_TREASURE)
                return 5;
            return -1;
        }
        if(Player.instance.weight_state() == 1)
        {
            if (selected_action == GO_TO_EXIT)
                return 5;
            return -5;
        }
        if(Player.instance.time_state() == 0)
        {
            if (selected_action == GO_TO_TARGET)
                return 5;
        }

        return 0;
    }

	double naive_reward_function(int selected_action)
    {
		if(Player.instance.flee_state() == 1)
		{
			if (selected_action == FLEE) {
				return 10;
			} else {
				return -5;
			}
		}
		if(Player.instance.visited_all_state() == 1)
		{
			if (selected_action == FLEE) {
				return 10;
			}  else {
				return -5;
			}
		}	
		if (Player.instance.at_exit_state () == 1) {
			if (selected_action == DROP_TREASURE) {
				return 10;
			} else {
				return -5;
			}
		}

		if(Player.instance.weight_state() == 1)
		{
			if (selected_action == GO_TO_EXIT) {
				return 5;
			} else	{
				return -5;
			}
		}
		if (Player.instance.at_open_treasure_state() == 1)
		{
			if (selected_action == PICK_UP_ITEM) {
				return 10;
			} else {
				return -5;
			}
		}
        return 0;
    }


    int getAction(int currentstate)
    {
        double maxVal = double.MinValue;
        int final_action = 0;

        for (int i = 0; i < POSSIBLE_MOVES; i++)
        {
            double qaction = Q[currentstate,i];
            if (qaction > maxVal)
            {
                maxVal = qaction;
                final_action = i;
            }
        }

        return final_action;
    }

    double getMaxQ(int state)
    {
        double maxVal = double.MinValue;

        for (int i = 0; i < POSSIBLE_MOVES; i++)
        {
            double qvalue = Q[state, i];
            if (qvalue > maxVal)
            {
                maxVal = qvalue;
            }
        }

        return maxVal;
    }

    public void EnvReset()
    {
        createModel(Q);
        record_observations();
        transform.position = start_pos;
        busy = false;
        Done = false;
        Player.instance.player_reset();
        Timer.instance.timer_reset();
        Astar.instance.reset();
        //Turret.instance.TurretReset();
    }

    void Tasks(int selected_action)
    {
        // go to selected target and collect gold
        if (selected_action == GO_TO_TARGET)
        {
            if (target != null)
            {
                Player.instance.set_current_treasure(target.gameObject);
                Grid.instance.final_path = Astar.instance.pathfinder(agent.transform.position, target.position);
                move.instance.autoMove(Grid.instance.final_path);
            }
            else
            {
                Player.instance.flee = true;
                busy = false;
            }
        }

        // go to exit
        if (selected_action == GO_TO_EXIT)
        {
            target = Astar.instance.exit.transform;
            Grid.instance.final_path = Astar.instance.pathfinder(agent.transform.position, target.position);
            move.instance.autoMove(Grid.instance.final_path);

            if (Vector3.Distance(agent.transform.position, target.position) <= 1)
            {
                busy = false; 
            }
        }

        // pick items with smart exchange behavior
        if (selected_action == PICK_UP_ITEM)
        {
			if(Player.instance.agent_type == NAIVE_AGENT) {
				Astar.instance.pick_up_loot(); 
			} else {
				Astar.instance.exchange_loot();
			}
        }

        // drops treasure
        if(selected_action == DROP_TREASURE)
        {
            Player.instance.drop_treasure_at_exit();
        }

        // skip target
        if (selected_action == SKIP_TARGET)
        {
            Astar.instance.skip_target();
        }

        if(selected_action == FLEE)
        {
            target = Astar.instance.exit.transform;
            Grid.instance.final_path = Astar.instance.pathfinder(agent.transform.position, target.position);
            move.instance.autoMove(Grid.instance.final_path);

            if (Vector3.Distance(agent.transform.position, target.position) <= 1)
            { 
                Player.instance.drop_treasure_at_exit();
                Done = true; 
            }
        }
    }


    void createModel(double[,] Q)
    {
        StringBuilder strb = new StringBuilder();

        for (int i = 0; i < STATES;i++)
        {
            for (int j = 0; j < POSSIBLE_MOVES;j++)
            {
                strb.Append(Q[i, j]+"\n");
            }
        }

        string path = selectModelPath();
        File.WriteAllText(path,strb.ToString());
    }

    void readModel()
    {
        string path = selectModelPath();
        using (StreamReader str = new StreamReader(path))
        {
            for (int i = 0; i < STATES;i++)
            {
                for (int j = 0; j < POSSIBLE_MOVES;j++)
                {
                    if (str.Peek() >= 0)
                    {
                        Q[i, j] = double.Parse(str.ReadLine());
                    }
                    else
                        return;
                }
            }
        }
    }

    void printModel()
    {
        for (int i = 0; i < STATES;i++)
        {
            for (int j = 0; j < POSSIBLE_MOVES;j++)
            {
                Debug.Log(Q[i,j]);
            }
        }
    }

    void record_observations()
    {
        StringBuilder strb = new StringBuilder();
        string training_path = selectTrainingPath();
        strb.AppendLine("Trail number: "+runcount+" , Gold collected: "+Player.instance.gold_value+" ,Reward: "+reward);
        File.AppendAllText(training_path, strb.ToString());
        runcount++;
    }


    //Learning function for sarsa
    int SarsaLearner()
    {
        float rnd = (float)(Random.Range(0, 101)) / 100;
        int currentstate = Player.instance.get_state();

        // random action according to epsilon-greedy 
        if (rnd < epsilon)
            action = Random.Range(0, POSSIBLE_MOVES);
        else
            action = getAction(currentstate);
        
        action = getAction(currentstate);
        Debug.Log("Sarsa Learning action: " + action);

        int nextstate = Player.instance.get_next_state(action);

        reward = sarsaRewardFunction(action);

        double currentQ = Q[currentstate, action];
        double maxQ = getMaxQ(nextstate);

        double q = (1 - alpha) * currentQ + alpha * (reward + (gamma * maxQ) - currentQ);

        Q[currentstate, action] = q;

        // epsilon decaying
        epsilon = 0.9 * epsilon;
        return action;
    }


    // SARSA
    double sarsaRewardFunction(int selected_action)
    {
		if(Player.instance.flee_state() == 1)
		{
			if (selected_action == FLEE)
				return 5;
			return -5;
		}

        if (Player.instance.detection_state() == 1)
        {
            if (selected_action == SKIP_TARGET)
                return 10;
            return -20;
        }

		if (Player.instance.visited_all_state() == 1)
		{
			if (selected_action == FLEE)
				return 10;
		}

		if (Player.instance.at_exit_state() == 1)
		{
			if (selected_action == DROP_TREASURE)
				return 5;
			return -5;
		}

        if (Player.instance.weight_state() == 1)
        {
            if (selected_action == GO_TO_EXIT)
                return 5;
            return -5;
        }
 
        if (Player.instance.at_open_treasure_state() == 1)
        {
            if (selected_action == PICK_UP_ITEM)
                return 5;
            return -5;
        }

        if (Player.instance.time_state() == 1)
        {
            if (selected_action == FLEE)
                return 10;
        }

        if (Player.instance.risk_state() == 1)
        {
            if (selected_action == FLEE)
                return 10;
        }

        return 0;
    }

    void makeEmpty()
    {
        string training_info = selectTrainingPath();
        File.WriteAllText(training_info, string.Empty);
    }

    string selectModelPath()
    {
        string path = smart_model_path;

        if (Player.instance.agent_type == SARSA_AGENT)
            path = sarsa_model_path;
        else if (Player.instance.agent_type == NAIVE_AGENT)
            path = naive_model_path;

        return path;
    }

    string selectTrainingPath()
    {
        string training_path = smart_train_info;

        if (Player.instance.agent_type == SARSA_AGENT)
            training_path = sarsa_train_info;
        else if (Player.instance.agent_type == NAIVE_AGENT)
            training_path = naive_train_info;

        return training_path;
    }

	int tactical_moves() {
		if (Player.instance.flee_state () == 1) {
			return FLEE;
		}
		if (Player.instance.visited_all_state () == 1) {
			return FLEE;
		}
		if (Player.instance.at_exit_state () == 1) {
			return DROP_TREASURE;
		}
		if (Player.instance.weight_state() == 1) {
			return GO_TO_EXIT;
		}
		if (Player.instance.at_open_treasure_state() == 1) {
			return PICK_UP_ITEM;
		}
		if (Player.instance.detection_state () == 1) {
			return SKIP_TARGET;
		}
		return GO_TO_TARGET;
	}
}
