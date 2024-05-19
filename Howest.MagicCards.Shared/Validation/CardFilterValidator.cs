using Howest.MagicCards.Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Howest.MagicCards.Shared.Validation
{
    public class CardFilterValidator : AbstractValidator<CardFilter>
    {
        public CardFilterValidator()
        {
            RuleSet("PageNumberValidation", () =>
            {
                RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");


                RuleFor(x => x.PageNumber)
                            .LessThanOrEqualTo(x => x.PageNumber)
                            .WithMessage("PageNumber must be less than or equal to PageNumber.");

            });
            RuleSet("PageNumberValidation", () =>
            {
                RuleFor(x => x.PageSize)
                    .GreaterThan(0)
                    .WithMessage("PageSize must be greater than 0.");

                RuleFor(x => x.PageSize)
                            .LessThanOrEqualTo(x => x.MaxPageSize)
                            .WithMessage("PageSize must be less than or equal to MaxPageSize.");

            });
        }
    }
}
