using Photon.Realtime;
using System;
using System.Diagnostics;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public static T GetProperty<T>(this Player player, string key)
    {
        PhotonHashtable property = player.CustomProperties;
        //key값이 있으면 해당 해시테이블에서 값을 가져와 T타입으로 변환해서 반환
        //없을 경우 해당 타입의 디폴트 값을 반환
        return property.ContainsKey(key) ? (T)property[key] : default(T);
    }

    public static void SetProperty<T>(this Player player, string str, T value)
    {
       // PhotonHashtable property = player.CustomProperties;                               
        PhotonHashtable property = new PhotonHashtable();                                    //set할때는 새로 만들고 해야함 이유는 각자 찾아보셈
        if (property.ContainsKey(str) == false) 
            //해당 프로퍼티가 포함되어있지 않을 경우 해시테이블을 생성
            property = new PhotonHashtable { { str, value } };
        else
            //포함되어있을 경우 값을 변경
            property[str] = value;
        //프로퍼티를 저장
        player.SetCustomProperties(property);
    }

    public static T GetProperty<T>(this Room room, string key)
    {
        PhotonHashtable property = room.CustomProperties;
        return property.ContainsKey(key) ? (T)property[key] : default(T);
    }

    public static void SetProperty<T>(this Room room, string str, T value)
    {
        PhotonHashtable property = new PhotonHashtable();
        if (property.ContainsKey(str) == false)
            /*PhotonHashtable*/
            property = new PhotonHashtable { { str, value } };
        else
            property[str] = value;
        // property.Add(str, value);
        room.SetCustomProperties(property);
    }

}
