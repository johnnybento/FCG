
using MediatR;

namespace FGC.Application.Catalog.Queries.ListPromotions;

public record ListPromotionsQuery(Guid JogoId) : IRequest<List<PromotionDto>>;
