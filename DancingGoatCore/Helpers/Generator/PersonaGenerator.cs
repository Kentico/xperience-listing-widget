using System;

using CMS.Personas;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Contains methods for generating sample personas.
    /// </summary>
    internal class PersonaGenerator
    {
        /// <summary>
        /// Represents Tony, the Cafe Owner.
        /// </summary>
        public const string PERSONA_CAFE_OWNER = "Tony_The_Cafe_Owner";

        /// <summary>
        /// Represents Martina, the Coffee Geek.
        /// </summary>
        public const string PERSONA_COFEE_GEEK = "Martina_The_Cofee_Geek";
        

        /// <summary>
        /// Generates two personas.
        /// </summary>
        public void Generate()
        {
            GenerateCoffeeGeekPersona();
            GenerateCafeOwnerPersona();
        }


        private void GenerateCoffeeGeekPersona()
        {
            var coffeeGeekPersona = PersonaInfo.Provider.Get(PERSONA_COFEE_GEEK);
            if (coffeeGeekPersona != null)
            {
                return;
            }

            coffeeGeekPersona = new PersonaInfo
            {
                PersonaDisplayName = "Martina, the Coffee Geek",
                PersonaName = PERSONA_COFEE_GEEK,
                PersonaDescription = "Martina is 28, she's an online entrepreneur and a foodie girl who likes to blog about her gastronomic experiences. \r\n\r\nThe preparation of her coffee has to be perfect.  She knows all the technical bits that go into the process. Not to leave things to chance, she also owns a professional espresso machine and a grinder.\r\n\r\nMartina drinks a cappuccino or a filtered coffee in the morning and then an espresso or machiato after each meal.",
                PersonaPointsThreshold = 15,
                PersonaPictureMetafileGUID = new Guid("8A3AF6F7-0914-42E1-9641-B7F2E04AED1B"),
                PersonaEnabled = true
            };
            PersonaInfo.Provider.Set(coffeeGeekPersona);
            coffeeGeekPersona.CreateScoreForPersona();
        }

        
        private void GenerateCafeOwnerPersona()
        {
            var coffeeOwnerPersona = PersonaInfo.Provider.Get(PERSONA_CAFE_OWNER);
            if (coffeeOwnerPersona != null)
            {
                return;
            }

            coffeeOwnerPersona = new PersonaInfo
            {
                PersonaDisplayName = "Tony, the Cafe Owner",
                PersonaName = PERSONA_CAFE_OWNER,
                PersonaDescription = "Tony has been running his own cafe for the last 7 years. He always looks at ways of improving the service he provides.\r\n\r\nHe offers coffee that he sources from several roasteries. In addition to that, he also sells brewing machines, accessories and grinders for home use.",
                PersonaPointsThreshold = 15,
                PersonaPictureMetafileGUID = new Guid("220C65BA-2CED-4347-9615-8CF69EAC20E5"),
                PersonaEnabled = true
            };
            PersonaInfo.Provider.Set(coffeeOwnerPersona);
            coffeeOwnerPersona.CreateScoreForPersona();
        }
    }
}