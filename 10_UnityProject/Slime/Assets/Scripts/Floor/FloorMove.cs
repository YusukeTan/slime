using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{

	[SerializeField]float moveX = 0;
	[SerializeField]float moveY = 0;
	[SerializeField]float speed = 0;
	[SerializeField]float waitTime = 0; //停止状態の待ち時間

	bool stop;
	float step;             //移動距離
	Vector3 origin;			//開始地点
	Vector3 destination;    //折り返し地点
	bool goBack = false;    //trueなら折り返し地点から開始地点に向かう

	void Start()
	{
		origin = transform.position;
		destination = new Vector3(origin.x - moveX, origin.y - moveY, origin.z);
	}

	void Update()
	{
		if (stop)
		{
			return;
		}

		step = speed * Time.deltaTime;

		if (!goBack)
		{
			transform.position = Vector3.MoveTowards(transform.position, destination, step);

			if (transform.position == destination) //現在地が折り返し地点に来たら
			{
				goBack = true;

				StartCoroutine(WaitTimer(waitTime)); //waitTimeだけ停止
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, origin, step);

			if (transform.position == origin) //現在地が開始地点に来たら
			{
				goBack = false;

				StartCoroutine(WaitTimer(waitTime)); //waitTimeだけ停止
			}
		}
	}

	protected IEnumerator WaitTimer(float waitTime)
	{
		stop = true;

		yield return new WaitForSeconds(waitTime);

		stop = false;
	}


}
