// AForge Machine Learning Library
//
// Copyright � Andrew Kirillov, 2007
// andrew.kirillov@gmail.com
//

namespace AForge.MachineLearning
{
    using System;

    /// <summary>
	/// QLearning learning algorithm
	/// </summary>
    /// 
    /// <remarks></remarks>
    /// 
	public class QLearning
	{
		// amount of possible states
		private int states;
		// amount of possible actions
		private int actions;
		// q-values
		private double[,] qvalues;

		// random number generator
		private Random rand = new Random( (int) DateTime.Now.Ticks );
		// exploration rate
		private double explorationRate = 0.1;
		// discount factor
		private double discountFactor = 0.95;
		// learning rate
		private double learningRate = 0.25;

        /// <summary>
        /// Amount of possible states
        /// </summary>
        /// 
        public int StatesCount
        {
            get { return states; }
        }

        /// <summary>
        /// Amount of possible actions
        /// </summary>
        /// 
        public int ActionsCount
        {
            get { return actions; }
        }

        /// <summary>
        /// Exploration rate
        /// </summary>
        /// 
        /// <remarks>Determines the rate (probability) of exploration steps.</remarks>
        /// 
		public double ExplorationRate
		{
            get { return explorationRate; }
            set { explorationRate = value; }
		}

        /// <summary>
        /// Learning rate
        /// </summary>
        /// 
        /// <remarks>The value determines the amount of updates Q-function receives
        /// during learning. The greater the value, the more updates the function receives.
        /// The lower the value, the less updates it receives.</remarks>
        /// 
		public double LerningRate
		{
			get { return LerningRate; }
			set { learningRate = value; }
		}

        /// <summary>
        /// Discount factor
        /// </summary>
        /// 
        /// <remarks>Discount factor for the expected summary reward.</remarks>
        /// 
        public double DiscountFactor
        {
            get { return discountFactor; }
            set { discountFactor = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QLearning"/> class
        /// </summary>
        /// 
        /// <param name="states">Amount of possible states</param>
        /// <param name="actions">Amount of possible actions</param>
        /// 
		public QLearning( int states, int actions )
		{
			this.states = states;
			this.actions = actions;

			qvalues = new double[states, actions];
		}

        /// <summary>
        /// Get next action from the specified state
        /// </summary>
        /// 
        /// <param name="state">Current state to get an action for</param>
        /// 
        /// <returns>Returns the action for the state</returns>
        /// 
		public int GetAction( int state )
		{
			// try to do exploration
			if ( rand.NextDouble( ) < explorationRate )
				return rand.Next( actions );

			// select the action with maximum expected reward
			double maxReward = double.MinValue;
			int action = 0;

			for ( int i = 0; i < actions; i++ )
			{
				if ( qvalues[state, i] > maxReward )
				{
					maxReward = qvalues[state, i];
					action = i;
				}
			}

			return action;
		}

        /// <summary>
        /// Update Q-function's value for the state-action pair
        /// </summary>
        /// 
        /// <param name="currentState">Curren state</param>
        /// <param name="nextState">Next state</param>
        /// <param name="action">Action, which lead from current to the next state</param>
        /// <param name="reward">Reward value, received by taking specified action from current state</param>
        /// 
		public void UpdateState( int currentState, int nextState, int action, double reward )
		{
			// find maximum expected summary reward from the next state
			double maxNextExpectedReward = double.MinValue;

			for ( int i = 0; i < actions; i++ )
			{
				if ( qvalues[nextState, i] > maxNextExpectedReward )
					maxNextExpectedReward = qvalues[nextState, i];
			}

			// update expexted summary reward of the current state
			qvalues[currentState, action] *= ( 1.0 - learningRate );
			qvalues[currentState, action] += ( learningRate * ( reward + discountFactor * maxNextExpectedReward ) );
		}
	}
}