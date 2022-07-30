using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Completed
{
	public class Loader : MonoBehaviour
	{

		public GameObject gameController;
		public GameObject soundManager;         //SoundManager prefab to instantiate.

		void Awake()
		{
			//gameController�̃C���X�^���X����������Ă��邩�m�F
			if (GameManager.instance == null)
			{
				//�Ȃ���ΐ�������
				Instantiate(gameController);
			}

			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			if (SoundManager.instance == null)

				//Instantiate SoundManager prefab
				Instantiate(soundManager);

		}

	}
}