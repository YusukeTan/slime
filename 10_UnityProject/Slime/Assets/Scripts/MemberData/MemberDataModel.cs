using System.Collections;
using System.Collections.Generic;
using MiniJSON; 

public class MemberDataModel
{
    public static List<MemberData> DeserializeFromJson(string sStrJson)
    {
        var ret = new List<MemberData>();

        // JSON�f�[�^�͔z�񂩂�n�܂�̂ŁADeserialize�i�f�R�[�h�j������Ƀ��X�g��      
        IList jsonList = (IList)Json.Deserialize(sStrJson);

        // ���X�g�̓��e�������^�̕ϐ��ɑ��
        foreach (IDictionary jsonOne in jsonList)
        {
            //�V���R�[�h��͊J�n
            var tmp = new MemberData();

            //�Y������L�[�������݂��邩���ׁA���݂�����ϐ��Ɋi�[����B
            if (jsonOne.Contains("Name"))
            {
                tmp.Name = (string)jsonOne["Name"]; //�v���C���[��
            }
            
            if (jsonOne.Contains("Time"))
            {
                tmp.Time = (string)jsonOne["Time"]; //�N���A����
                
            }

            //���ʂɒǉ�
            ret.Add(tmp);
        }

        return ret;
    }
}