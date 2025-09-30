using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.InformationModel.Queries.GetAll;

public class GetAllInformationModelsRequestQuery:IRequest<BaseResponse<List<InformationModelResponse>>>
{

}
