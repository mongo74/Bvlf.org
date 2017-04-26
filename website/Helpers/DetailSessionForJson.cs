namespace bvlf_v2.Helpers
{
    public class DetailSessionForJson : SessionForJson
    {
        public string at_Type { get; set; }
        public string at_StartTime { get; set; }
        public string at_EndTime { get; set; }
        public int at_Maxplaces { get; set; }
        public string at_Description { get; set; }
        public string at_Doelgroep { get; set; }
        public string at_Metdesteunvan { get; set; }
        public string at_Speaker { get; set; }
        public string at_SpeakerInfo { get; set; }
        public bool at_Iseenherhaling { get; set; }
    }
}