using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


[DefaultExecutionOrder(-4)]
public class TitleManager : MonoBehaviour
{

    [SerializeField] Text inputName = default;
    [SerializeField] Text errorText = default;      //���O�����͎��̃G���[�e�L�X�g
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject rankingPanel;
    [SerializeField] GameObject howtoPlayPanel;
    [SerializeField] GameObject rankingItemBase;    //�����L���O���\�������X�N���[���r���[�R���e���c
    [SerializeField] GameObject rankingItem;
    [SerializeField] Text _displayField;            //�u�����L���O�擾���v�ƕ\������e�L�X�g

    private List<MemberData> _memberList;
    private SoundManager soundManager;
    private MoveSceneManager moveSceneManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        moveSceneManager = MoveSceneManager.Instance;
    }


    //�X�^�[�g�{�^���̏���
    void StartGame()
    {
        if ("" == inputName.text)
        {
            errorText.text = "���O�����Ă���X�^�[�g���Ă�";
        }
        else
        {
            //���͂��ꂽ���O��ۑ�
            PlayerPrefs.SetString("NAME", inputName.text);
            PlayerPrefs.Save();
            moveSceneManager.GoToMain();
            soundManager.PlayMainBGM();
        }
    }

    public void OnClickRanking()
    {
        titlePanel.SetActive(false);
        rankingPanel.SetActive(true);

        _displayField.text = "�f�[�^�擾��...";
        GetJsonFromWebRequest();
    }

    public void OnClickHowtoPlay()
    {
        titlePanel.SetActive(false);
        howtoPlayPanel.SetActive(true);

    }


    void ShowMemberList()
    {
        string sStrOutput = "";

        if (null == _memberList)
        {
            sStrOutput = "no list !";
        }
        else
        {
            //���X�g�̓��e��\��
            int positionCount = 0;
            int num = 1;
            _displayField.text = "";

            foreach (MemberData memberOne in _memberList)
            {
                GameObject rankingObject = Instantiate(rankingItem, rankingItemBase.transform.position, Quaternion.identity, rankingItemBase.transform);
                rankingObject.transform.SetParent(rankingItemBase.transform);   //�X�N���[���r���[�̃R���e���c��e�ɂ���

                rankingObject.transform.Find("Rank").GetComponent<Text>().text = num.ToString();    //����
                rankingObject.transform.Find("Score").GetComponent<Text>().text = memberOne.Time;   //�N���A����                
                rankingObject.transform.Find("Name").GetComponent<Text>().text = memberOne.Name;    //���O

                //2�ʈȍ~�͉���30�����炵�ĕ\������悤��
                Vector3 itemOffset = new Vector3(0, -30f * positionCount, 0);
                rankingObject.transform.GetComponent<RectTransform>().position += itemOffset;

                positionCount++;
                num += 1;
            }
        }

        _displayField.text = sStrOutput;
    }

    private void GetJsonFromWebRequest()
    {
        //  json �f�[�^�擾�����N�G�X�g����
        StartCoroutine(
            DownloadJson(
                CallbackWebRequestSuccess, // API�R�[�������������ۂɌĂ΂��֐�
                CallbackWebRequestFailed // API�R�[�������s�����ۂɌĂ΂��֐�
            )
        );
    }

    private void CallbackWebRequestSuccess(string response)
    {
        //Json �̓��e�� MemberData�^�̃��X�g�Ƃ��ăf�R�[�h����B
        _memberList = MemberDataModel.DeserializeFromJson(response);
        ShowMemberList();

    }

    public void OnClickReturn()
    {
        moveSceneManager.GoToTitle();
    }

    private void CallbackWebRequestFailed()
    {
        _displayField.text = "�����L���O�擾���s���܂���";
    }

    private IEnumerator DownloadJson(Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/rankingapi/ranking/getRanking");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            //���X�|���X�G���[�̏ꍇ
            Debug.LogError(www.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (www.isDone)
        {
            // ���N�G�X�g�����̏ꍇ
//            Debug.Log($"Success:{www.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(www.downloadHandler.text);
            }
        }
    }

}
