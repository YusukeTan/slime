public class MemberData
{
    public string Name { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }

    public MemberData()
    {
        Name = ""; //プレイヤー名
        Time = ""; //クリア時間
        Date = ""; //登録日付
    }
}