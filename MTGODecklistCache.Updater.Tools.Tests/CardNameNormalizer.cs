using FluentAssertions;
using NUnit.Framework;

namespace MTGODecklistCache.Updater.Tools.Tests
{
    public class CardNormalizerTests
    {
        [Test]
        public void ShouldRemoveLeadingSpaces()
        {
            CardNameNormalizer.Normalize(" Arclight Phoenix").Should().Be("Arclight Phoenix");
            CardNameNormalizer.Normalize("  Arclight Phoenix").Should().Be("Arclight Phoenix");
        }

        [Test]
        public void ShouldRemoveTrailingSpaces()
        {
            CardNameNormalizer.Normalize("Arclight Phoenix ").Should().Be("Arclight Phoenix");
            CardNameNormalizer.Normalize("Arclight Phoenix  ").Should().Be("Arclight Phoenix");
        }

        [Test]
        public void ShouldNormalizeSplitCards()
        {
            CardNameNormalizer.Normalize("Fire").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire/Ice").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire / Ice").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire//Ice").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire // Ice").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire///Ice").Should().Be("Fire // Ice");
            CardNameNormalizer.Normalize("Fire /// Ice").Should().Be("Fire // Ice");
        }

        [Test]
        public void ShouldNormalizeFlipCards()
        {
            CardNameNormalizer.Normalize("Akki Lavarunner").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner/Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner / Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner//Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner // Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner///Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
            CardNameNormalizer.Normalize("Akki Lavarunner /// Tok-Tok, Volcano Born").Should().Be("Akki Lavarunner");
        }

        [Test]
        public void ShouldNormalizeAdventureCards()
        {
            CardNameNormalizer.Normalize("Brazen Borrower").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower/Petty Theft").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower / Petty Theft").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower//Petty Theft").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower // Petty Theft").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower///Petty Theft").Should().Be("Brazen Borrower");
            CardNameNormalizer.Normalize("Brazen Borrower /// Petty Theft").Should().Be("Brazen Borrower");
        }

        [Test]
        public void ShouldNormalizeDualFaceCards()
        {
            CardNameNormalizer.Normalize("Delver of Secrets").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets/Insectile Aberration ").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets / Insectile Aberration ").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets//Insectile Aberration ").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets // Insectile Aberration ").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets///Insectile Aberration ").Should().Be("Delver of Secrets");
            CardNameNormalizer.Normalize("Delver of Secrets /// Insectile Aberration ").Should().Be("Delver of Secrets");
        }

        [Test]
        public void ShouldFixAetherVial()
        {
            CardNameNormalizer.Normalize("Æther Vial").Should().Be("Aether Vial");
        }

        [Test]
        public void ShouldFixAetherInDualFaceCards()
        {
            CardNameNormalizer.Normalize("Invasion of Kaladesh // Ætherwing, Golden-Scale Flagship").Should().Be("Invasion of Kaladesh");
        }

        [Test]
        public void ShouldFixUniversesWithinCards()
        {
            CardNameNormalizer.Normalize("Rick, Steadfast Leader").Should().Be("Greymond, Avacyn's Stalwart");
        }

        [Test]
        public void ShouldFixUniversesWithinDfcCards()
        {
            CardNameNormalizer.Normalize("Hawkins National Laboratory").Should().Be("Havengul Laboratory");
            CardNameNormalizer.Normalize("Hawkins National Laboratory // The Upside Down").Should().Be("Havengul Laboratory");
        }

        [Test]
        public void ShouldConvertAlchemyBuffsAndNerfsToRegularCard()
        {
            CardNameNormalizer.Normalize("A-Dragon's Rage Channeler").Should().Be("Dragon's Rage Channeler");
        }

        [Test]
        public void ShouldConvertAlchemyAdventureCardsToNormalCard()
        {
            CardNameNormalizer.Normalize("A-Blessed Hippogriff // Tyr's Blessing").Should().Be("Blessed Hippogriff");
        }
    }
}