
public static class PropertyExtension
{

    //key값이 있으면 해당 해시테이블에서 값을 가져와 T타입으로 변환해서 반환
    //없을 경우 해당 타입의 디폴트 값을 반환
    //public static T GetProperty<T>(this Player player, string key)
    //{
    //    ExitGames.Client.Photon.
    //        Hashtable property = player.CustomProperties;
    //    return property.ContainsKey(key) ? (T)property[key] : default;
    //}
    //public static T GetProperty<T>(this Room room, string key)
    //{
    //    ExitGames.Client.Photon.
    //        Hashtable property = room.CustomProperties;
    //    return property.ContainsKey(key) ? (T)property[key] : default;
    //}

    //public static void SetProperty<T>(this Player player, string key, T value)
    //{
    //    ExitGames.Client.Photon.
    //        Hashtable hashTable = new() { { key, value } };
    //    player.SetCustomProperties(hashTable);
    //}

    //public static void SetProperty<T>(this Room room, string key, T value)
    //{
    //    ExitGames.Client.Photon.
    //        Hashtable hashTable = new() { { key, value } };
    //    room.SetCustomProperties(hashTable);
    //}

    //set할때는 새로 만들고 해야함 이유는 각자 찾아보셈
    //(답 :
    //일관성과 무결성을 보장하는 방법 중 하나 ,
    //생성하여 대입은 내부적으로 감지하여 변경 사항 알림,
    //코드의 명확성을 높임,
    //기존 해시 테이블에 직접 변경을 가하면 의도치 않은 사이드 이펙트가 발생)

    //포함되어있을 경우 값을 변경
    //포함되어있지 않을 경우 해시테이블을 생성
    //프로퍼티를 저장
}
