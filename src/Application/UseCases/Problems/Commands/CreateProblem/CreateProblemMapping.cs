//using Domain.Entities;
//using Mapster;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.UseCases.Problems.Commands.CreateProblem;

//public class CreateProblemMapping : IRegister
//{
//    public void Register(TypeAdapterConfig config)
//    {
//        // Mapping từ Command -> Entity
//        config.NewConfig<CreateProblemCommand , Problem>()
//              .Ignore(dest => dest.Id); // Id được sinh trong constructor
//    }
//}

