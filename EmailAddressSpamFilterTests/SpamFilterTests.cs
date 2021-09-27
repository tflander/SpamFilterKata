using FluentAssertions;
using Xunit;

namespace EmailAddressSpamFilterTests
{
    public class SpamFilterTests
    {
        [Fact]
        public void VerifySpamEmailAddresses()
        {
            var sf = new EmailAddressSpamFilter.SpamFilter();
            sf.LoadSpamEmailAddresses("Spam.txt");
            
            sf.IsSpam("trustme@evilspammer.com").Should().BeTrue();
            sf.IsSpam("buyworthlessproduct@iwantyourmoney.com").Should().BeTrue();
            sf.IsSpam("joinus@pyramidscheme.com").Should().BeTrue();

            sf.IsSpam("anyone@accenture.com").Should().BeFalse();
        }
    }
}