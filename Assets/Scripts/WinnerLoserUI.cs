using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerLoserUI : MonoBehaviour {
	// Components
	[SerializeField] private GameObject go_player0Lose;
	[SerializeField] private GameObject go_player1Lose;

	public void Reset () {
		go_player0Lose.SetActive(false);
		go_player1Lose.SetActive(false);
	}

	public void OnGameOver (int losingPlayerIndexImSorryForThisVariableNamePleaseChangeMeOrDontYouLiveYourLifeAsYouDoByTheWayYouRockKeepItUpYeahYouYeah) {
		int smallerVariableName = losingPlayerIndexImSorryForThisVariableNamePleaseChangeMeOrDontYouLiveYourLifeAsYouDoByTheWayYouRockKeepItUpYeahYouYeah;
		int smallest = smallerVariableName;
		int s = smallest;

		go_player0Lose.SetActive (s==0);
		go_player1Lose.SetActive (s!=0);
		// Why aren't there more comments after code
	}
}
