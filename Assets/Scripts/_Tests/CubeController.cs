using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour, ICommand {

	private UserInput userInput;

	void Awake()
	{
		userInput = GetComponent<UserInput>();
	}

	private void Update()
	{
		Command command = userInput.GetCommand();

		if (command != null)
		{
			command.Execute(this);
		}
	}

	public void Fire()
	{
		Debug.Log("Firing");
	}
}
