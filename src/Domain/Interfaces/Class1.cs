using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    internal class Class1
    {
        //  hiện tại khá là đắn đo về việc nên tạo folder interfaces ở layer domain ko?

        // kiểu như nếu ở đây tạo thì sẽ có các interfaces của repositories tương ứng vói feat và các table được scaffold
        //  tạm thời ở đây sẽ có các repository cho generic chia ra theo 2 trường phái READ & WRITE
        //      VÌ NẾU MUỐN áp dụng CQRS thì cần chia generic repo sang read vs write
        //      điểm mạnh là mấy cái crud thì tiện ko cần impl lại
        //      ngoài ra, thì ko cần tạo repo nếu repo đó thuần crud, đó regist ở program.cs dạng assembly là nó tự động hoá hết
    }
}
