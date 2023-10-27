using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class RollPaper : Item
{
    //일반몹 죽으면 확률로 드랍 / UI에 띄워야하고 / 매 스테이지에 선택지 선택시 차감
    //  이건 논의            인벤토리매니저로 해결       이건 만들어야함
    bool CanTest=true;
    float TestTime =1.0f;
    
    protected override void Update()
    {   
        base.Update();
        if(Input.GetKey(KeyCode.T)&&CanTest)
        {
            UseRollPaper(2);
        }
    }
    //일단 이 메소드에서 보유 개수보다 많이 사용하려는걸 막음
    //근데 두루마리는 선택지에서 클릭하는건데 선택지 클릭할 메소드에서 if문으로 검사하는게 낫지않나?
    //아니면 애초에 보유 두루마리개수보다 적게 사용하는 선택지만 나오게하던가
    public void UseRollPaper(int count)
    {   
        if(Inventory.rollPaperCount>=count)
        {
            Inventory.rollPaperCount-=count;
            Inventory.UpdateRollPaperCount(Inventory.rollPaperCount);
            CanTest=false;
            StartCoroutine(RollTest());
        }
    }

    public IEnumerator RollTest()
    {
    yield return new WaitForSeconds(TestTime);
    CanTest = true;
    }

}
