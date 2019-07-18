using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace UME
{
	[RequireComponent(typeof (Canvas))]
	public class UIControl : MonoBehaviour
	{
		[HideInInspector]
		public List<GameMessage> MessageBoards = new List<GameMessage>();
		[HideInInspector]
		public List<UIData> UIDataSources = new List<UIData>();
		//private Canvas m_Canvas;

		private bool startBtn = false;
		private bool pauseBtn = false;
		private bool continueBtn = false;
		private bool restartBtn = false;

		private bool canContinue = false;
		private bool pause = false;
		private bool play = true;
		private bool start = true;
		private bool stop = false;
		float delayTimer = 0f;
		// Use this for initialization
		void Start()
		{

			for(int i=0; i < MessageBoards.Count; i++) {
				Time.timeScale = 0;
				string options = "\n\npress <s> to start..\n";
				MessageBoards[i].UpdateValue (GameState.start, options, null);
			}

		}


		// Update is called once per frame
		void Update ()
		{
			delayTimer -= Time.deltaTime;
			
			pauseBtn = Input.GetKeyDown ("p");
			if (start) {
				startBtn = Input.GetKeyDown ("s");
			}
			if ( play == false) {
				restartBtn = Input.GetKeyDown ("r");
				continueBtn = Input.GetKeyDown ("c");
			}

			if (pauseBtn){
				pause = !pause;
				if (pause == true){
					DisplayMessage("Pause <p>");
					Time.timeScale = 0;
					play = false;
					stop = false;
				}
				if (pause == false){
					DisplayMessage("");
					Time.timeScale = 1;
					play = true;
					stop = false;
				}
			}

			if (continueBtn && canContinue){
				DisplayMessage("");

				int next_level_idx = SceneManager.GetActiveScene().buildIndex+1;
				if (next_level_idx < SceneManager.sceneCountInBuildSettings) {
					SceneManager.LoadScene (next_level_idx);
				} else {
					SceneManager.LoadScene (0);
				}
				Time.timeScale = 1;
				play = true;
				stop = false;
			}

			if (restartBtn || startBtn ){
				DisplayMessage("");
				play=true;
				start=false;
				stop = false;

				if (restartBtn)
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

				
				Time.timeScale=1;

			}
			if (stop && delayTimer < 0) {
				Time.timeScale = 0;
			}
			startBtn = false;
			pauseBtn = false;
			continueBtn = false;
			restartBtn = false;
			//quitBtn = false;
		}


		public void UpdateState(UITriggerType type, string value, float duration,  GameObject target=null){

			Animator m_anim = target.GetComponent<Animator> ();
			switch (type) {
			case UITriggerType.win:
				UpdateGameMessage (GameState.win, target, value);
				break;
			case UITriggerType.lose:
				if (m_anim) {
					m_anim.SetBool ("Death", true);
					delayTimer = m_anim.GetCurrentAnimatorClipInfo (0) [0].clip.length;
					if (target.GetComponent<Rigidbody2D> () != null)
						//target.GetComponent<Rigidbody2D> ().velocity = new Vector2(0f,0f);
						target.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;

				}

				UpdateGameMessage (GameState.lose, target, value);
				break;

			case UITriggerType.message:
				DisplayMessage (value, target, duration);
				break;

			case UITriggerType.time:
				if ((float)Int32.Parse (value) > 0)
					m_anim.SetBool ("Happy", true);
				else
					m_anim.SetBool ("Sad", false);
				UpdateBoard (type, value, target);
				break;
				
			case UITriggerType.score:
				if (Int32.Parse (value) > 0)
					m_anim.SetBool ("Happy", true);
				else
					m_anim.SetBool ("Sad", false);
				UpdateBoard (type, value, target);
				break;

			case UITriggerType.health:
				if (Int32.Parse (value) > 0)
					m_anim.SetBool ("Happy", true);
				else
					m_anim.SetBool ("Sad", false);
				UpdateBoard (type, value, target);
				break;

			default:
				UpdateBoard (type, value, target);
				break;
			}

		}

		private void UpdateBoard(UITriggerType type, string value, GameObject target=null){

			bool match = false;
			//filter the boards 
			List<UIData> uiBoards = (from UIData data in UIDataSources
				where data.GetBoardType().ToString() == type.ToString()
				select data).ToList ();
			try
			{
				if (target != null)
				{
					for (int i = 0; i < uiBoards.Count; i++)
					{
						UIData data = uiBoards[i];
						if (uiBoards[i].gameObject != target)
							continue;
						match = true;
						switch (type)
						{
						case UITriggerType.time:
							data.UpdateValue((float)Int32.Parse(value));
							break;
						default:
							data.UpdateValue(Int32.Parse(value));
							break;
						}

					}

				}
				if (!match)
				{
					for (int i = 0; i < uiBoards.Count; i++)
					{
						switch (type)
						{
						case UITriggerType.time:
							uiBoards[i].UpdateValue((float)Int32.Parse(value));
							break;
						default:
							uiBoards[i].UpdateValue(Int32.Parse(value));
							break;
						}

					}
				}
			}
			catch (FormatException e)
			{
				Debug.Log(string.Format("{0}: {1}", e, value));
			}

		}

		public void UpdateGameMessage(GameState state, GameObject target = null, string value = null){
			//TODO: make this parent specific? Let other players keep playing

			string options = "";
			switch (state) 
			{
			case GameState.lose:
				stop = true;
				options = "\n===========\nrestart <r>";
				play = false;
				break;

			case GameState.win:
				stop = true;
				int currentIndex = SceneManager.GetActiveScene ().buildIndex;
				int build_count = SceneManager.sceneCountInBuildSettings;
				options = "\n===========\n";			
				if (currentIndex + 1 < build_count) {
					options += "continue <c>\n";
					canContinue = true;
				}
				options += "replay <r>\n";
				play = false;
				break;

			}

			//choose local game message
			if (target == null)
				GetComponentInChildren<GameMessage>().UpdateValue(state, options, value);

			for(int i=0;i < MessageBoards.Count;i++) {
				MessageBoards[i].UpdateValue (state, options, value);
			}


		}

		public void DisplayMessage(string msg, GameObject parent=null, float time=0f){

			for (int i=0; i < MessageBoards.Count; i++) {
				MessageBoards[i].UpdateValue(msg);
				if (time > 0f)
				{
					MessageBoards[i].timed = true;
					MessageBoards[i].displayTime = time;
				}
			}


		}



	}
}
