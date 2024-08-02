using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;

namespace Howest.MagicCards.Shared.DTO
{
    public record CardDetailDTO
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string ManaCost { get; init; }
        public string ConvertedManaCost { get; init; }
        public string Type { get; init; }
        public string RarityCode { get; init; }
        public string SetCode { get; init; }
        public string Text { get; init; }
        public string Flavor { get; init; }
        public long? ArtistId { get; init; }
        public string Number { get; init; }
        public string Power { get; init; }
        public string Toughness { get; init; }
        public string Layout { get; init; }
        public int? MultiverseId { get; init; }
        public string OriginalImageUrl { get; init; }
        public string Image { get; init; }
        public string OriginalText { get; init; }
        public string OriginalType { get; init; }
        public string MtgId { get; init; }
        public string Variations { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public string ArtistName { get; init; }
       
    }
}
