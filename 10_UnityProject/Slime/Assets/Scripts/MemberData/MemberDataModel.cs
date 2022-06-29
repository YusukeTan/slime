using System.Collections;
using System.Collections.Generic;
using MiniJSON; 

public class MemberDataModel
{
    public static List<MemberData> DeserializeFromJson(string sStrJson)
    {
        var ret = new List<MemberData>();

        // JSONデータは配列から始まるので、Deserialize（デコード）した後にリストへ      
        IList jsonList = (IList)Json.Deserialize(sStrJson);

        // リストの内容を辞書型の変数に代入
        foreach (IDictionary jsonOne in jsonList)
        {
            //新レコード解析開始
            var tmp = new MemberData();

            //該当するキー名が存在するか調べ、存在したら変数に格納する。
            if (jsonOne.Contains("Name"))
            {
                tmp.Name = (string)jsonOne["Name"]; //プレイヤー名
            }
            
            if (jsonOne.Contains("Time"))
            {
                tmp.Time = (string)jsonOne["Time"]; //クリア時間
                
            }

            //結果に追加
            ret.Add(tmp);
        }

        return ret;
    }
}