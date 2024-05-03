using EdmentumDAL;
using EdmentumDAL.ModelClass;
using Newtonsoft.Json;
using System.Text;

namespace EdmentumBLL.Manager
{
    public class MeetingManager
    {
        private readonly EdmentumContext _context;
        private readonly HttpClient _httpClient;
        public MeetingManager(EdmentumContext context, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _context = context;
        }
        public async Task<HttpResponseMessage> CreateMeetingAsync(MeetingRequest request)
        {
            try
            {
                string jsonRequest = JsonConvert.SerializeObject(request);
                //For now hard coding it for POC
                //string authHeader = "R2N2ZzNRZ1RjNmpqZHRaVi4xNzE0NjgwNjEwMjYyOjdhMzNlY2ZhN2Q5YjBhZGU0ZWViNDEzMTk5ZTVkZjc1YmFiZjhiMGU5NDIyMWMyZTg0MzlhODQ1ZDIwOTU4NWY=";
                string authHeader = "R2N2ZzNRZ1RjNmpqZHRaVi4xNzE0NzU3MTAyNzA1OjI3ZjEyNjFiODI2NmVkZThhMzk2N2U2ZjZkYzFlZTcxZTFiOWVkYjA1MjE3YTliMmFkMjY0Y2E0MTkzYWM2ODQ=";


                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                return await _httpClient.PostAsync("https://vcaas.hilink.co/api/v2/meeting-center/meetings", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void AddMeeting(Meeting meeting)
        {
            // Add the meeting details to the database
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
        }

        public Meeting GetMeetingById(long meetingId)
        {
            // Retrieve the meeting from the database by its ID
            return _context.Meetings.FirstOrDefault(m => m.MeetingId == meetingId);
        }

        public IEnumerable<Meeting> GetAllMeetings()
        {
            // Retrieve all meetings from the database
            return _context.Meetings.ToList();
        }
    }
}
