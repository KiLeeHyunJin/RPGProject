using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData
{
    public class CharacterServerData
    {
        public string nickName; //�г���
        public short jobLv;     //����, ����(1����Ʈ ��)

        public long cloth;      //cġ��
        public int skin;        //����    (1����Ʈ ��)

        public int speed;       //�̵��ӵ�, ������,  ���ݼӵ�, �߰�Ÿ)
        public int atck;        //��������, ��������, ġ�� ������)
        public int other;       //���߷�, ġ���, ����, ȸ�Ƿ�, 

        public int ability;    //���� (1����Ʈ ��)

        public long point;      //�ɷ�ġ, 0��, 1��, 2��, 3��, 4��   
    }

    public class InventoryServerData
    {
        public InventoryServerData()
        {
            consume = new();
            ect = new();
            equip = new();
            consume = new();
        }
        public List<ItemServerData> consume;
        public List<ItemServerData> ect;
        public List<ItemServerData> equip;
        public long money;
    }

    public class ItemServerData
    {
        public string itemName; //�̸�

        public long code;       //����, ī�װ�, �̹���, ��ũ���ͺ�
        public long itemData;   //Ÿ��, ����, ����, �ۼ�,

        public int limitStat;   //��, ��ø, ����, ��,

        public long addAbility; //��, ��ø, ����, ��, ����, ����, ���, �̼�, 
        public long addStat;    //��, ��ø, ����, ��, ����, ����, ���, �̼�, 
        public long upgradeStat;//��, ��ø, ����, ��, ����, ����, ���, �̼�, 
    }

}
