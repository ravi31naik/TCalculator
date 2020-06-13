using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkingCalculator.Model;
using Xamarin.Essentials;

namespace TalkingCalculator
{
    public class TextToSpeechCalc
    {
        public Locale localePicked { get; set; } = null;
        TextToSpeechHelper textToSpeechHelper = new TextToSpeechHelper();
        public TextToSpeechCalc()
        {
            Task.Run(() =>
            {
                localePicked = TextToSpeech.GetLocalesAsync().Result.FirstOrDefault();
            });
        }
        public async Task SayNumberOutLoudAsync(string number)
        {
            var settings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = localePicked
            };

            await TextToSpeech.SpeakAsync(textToSpeechHelper.ConvertNumberToEnglish(number), settings);
        }

        public async Task<List<Locale>> GetAllLocaleAsync()
        {
            var asycResult = await TextToSpeech.GetLocalesAsync();
            return asycResult.ToList();
        }
        public List<string> GetLocalsList()
        {
            var result = GetAllLocaleAsync().Result;
            List<string> languages = new List<string>();

            foreach (var lang in result)
            {
                languages.Add(lang.Country + " " + lang.Language + " " + lang.Name);
            }
            return languages;
        }
    }
}