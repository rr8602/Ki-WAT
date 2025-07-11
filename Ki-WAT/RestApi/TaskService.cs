using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LETInterface
{
    public class TaskService
    {
        private readonly string _server;
        private readonly int _port;
        private readonly HttpClient _client;

        public TaskService(string server, int port)
        {
            try
            {
                _server = server ?? "localhost"; // null인 경우 기본값 사용
                _port = (port <= 0 || port > 65535) ? 8080 : port; // 유효하지 않은 포트인 경우 기본값 사용
                
                _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(30); // 30초 타임아웃 설정
                
                Logger.WriteLog($"TaskService 초기화 완료: {_server}:{_port}", true);
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskService 생성자 오류: {ex.Message}", true);
                // 기본값으로 초기화
                _server = "localhost";
                _port = 8080;
                _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(30);
            }
        }

        public string Url(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    Logger.WriteLog("URL 생성 오류: 경로가 null이거나 비어있습니다.", true);
                    return $"http://{_server}:{_port}/";
                }
                
                return $"http://{_server}:{_port}{path}";
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"URL 생성 오류: {ex.Message}", true);
                return $"http://{_server}:{_port}/";
            }
        }

        public HttpResponseMessage TaskAddNew()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Url("/tasks"));
                string reqLog = Logger.HttpMessageToLogString(request);
                Logger.WriteLog(reqLog, true); // 로그 기록

                var response = _client.SendAsync(request).Result;
                string respLog = Logger.HttpMessageToLogString(response);
                Logger.WriteLog(respLog, true); // 응답 로그

                return response;
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (TaskAddNew): {ex.Message}", true);
                return CreateErrorResponse("서버 연결 오류");
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (TaskAddNew): {ex.Message}", true);
                return CreateErrorResponse("서버 응답 시간 초과");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskAddNew 오류: {ex.Message}", true);
                return CreateErrorResponse("작업 추가 중 오류 발생");
            }
        }

        public HttpResponseMessage TaskSpecify(string path, object content)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    Logger.WriteLog("TaskSpecify 오류: 경로가 null이거나 비어있습니다.", true);
                    return CreateErrorResponse("경로가 유효하지 않습니다.");
                }
                
                if (content == null)
                {
                    Logger.WriteLog("TaskSpecify 오류: 콘텐츠가 null입니다.", true);
                    return CreateErrorResponse("콘텐츠가 유효하지 않습니다.");
                }

                var json = JsonSerializer.Serialize(content);

                var request = new HttpRequestMessage(HttpMethod.Put, Url(path));
                string reqLog = Logger.HttpMessageToLogString(request);
                Logger.WriteLog(reqLog, true);

                var response = _client.PutAsync(Url(path), new StringContent(json, Encoding.UTF8, "application/json")).Result;
                string respLog = Logger.HttpMessageToLogString(response);
                Logger.WriteLog(respLog, true);

                return response;
            }
            catch (JsonException ex)
            {
                Logger.WriteLog($"JSON 직렬화 오류 (TaskSpecify): {ex.Message}", true);
                return CreateErrorResponse("데이터 직렬화 오류");
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (TaskSpecify): {ex.Message}", true);
                return CreateErrorResponse("서버 연결 오류");
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (TaskSpecify): {ex.Message}", true);
                return CreateErrorResponse("서버 응답 시간 초과");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskSpecify 오류: {ex.Message}", true);
                return CreateErrorResponse("작업 지정 중 오류 발생");
            }
        }

        public HttpResponseMessage TaskGet(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    Logger.WriteLog("TaskGet 오류: 경로가 null이거나 비어있습니다.", true);
                    return CreateErrorResponse("경로가 유효하지 않습니다.");
                }

                var request = new HttpRequestMessage(HttpMethod.Get, Url(path));
                string reqLog = Logger.HttpMessageToLogString(request);
                Logger.WriteLog(reqLog, true);

                var response = _client.SendAsync(request).Result;
                string respLog = Logger.HttpMessageToLogString(response);
                Logger.WriteLog(respLog, true);

                return response;
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (TaskGet): {ex.Message}", true);
                return CreateErrorResponse("서버 연결 오류");
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (TaskGet): {ex.Message}", true);
                return CreateErrorResponse("서버 응답 시간 초과");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskGet 오류: {ex.Message}", true);
                return CreateErrorResponse("작업 조회 중 오류 발생");
            }
        }

        public HttpResponseMessage TaskDeleteAll()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, Url("/tasks"));
                string reqLog = Logger.HttpMessageToLogString(request);
                Logger.WriteLog(reqLog, true);

                var response = _client.SendAsync(request).Result;
                string respLog = Logger.HttpMessageToLogString(response);
                Logger.WriteLog(respLog, true);

                return response;
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (TaskDeleteAll): {ex.Message}", true);
                return CreateErrorResponse("서버 연결 오류");
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (TaskDeleteAll): {ex.Message}", true);
                return CreateErrorResponse("서버 응답 시간 초과");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskDeleteAll 오류: {ex.Message}", true);
                return CreateErrorResponse("작업 삭제 중 오류 발생");
            }
        }

        public string GetResultByUid(string uid)
        {
            try
            {
                if (string.IsNullOrEmpty(uid))
                {
                    Logger.WriteLog("GetResultByUid 오류: UID가 null이거나 비어있습니다.", true);
                    return "<error>UID가 유효하지 않습니다.</error>";
                }

                var response = _client.GetAsync(Url($"/results_by_uid/{uid}.xml")).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = $"UID {uid}로 결과 조회 실패. 상태 코드: {response.StatusCode}";
                    Logger.WriteLog(errorMsg, true);
                    return $"<error>{errorMsg}</error>";
                }
                
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (GetResultByUid): {ex.Message}", true);
                return "<error>서버 연결 오류</error>";
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (GetResultByUid): {ex.Message}", true);
                return "<error>서버 응답 시간 초과</error>";
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"GetResultByUid 오류: {ex.Message}", true);
                return "<error>UID로 결과 조회 중 오류 발생</error>";
            }
        }

        public string GetLastResult()
        {
            try
            {
                var response = _client.GetAsync(Url("/last_result.xml")).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = $"마지막 결과 조회 실패. 상태 코드: {response.StatusCode}";
                    Logger.WriteLog(errorMsg, true);
                    return $"<error>{errorMsg}</error>";
                }
                
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLog($"HTTP 요청 오류 (GetLastResult): {ex.Message}", true);
                return "<error>서버 연결 오류</error>";
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteLog($"요청 타임아웃 (GetLastResult): {ex.Message}", true);
                return "<error>서버 응답 시간 초과</error>";
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"GetLastResult 오류: {ex.Message}", true);
                return "<error>마지막 결과 조회 중 오류 발생</error>";
            }
        }

        // 오류 응답을 생성하는 헬퍼 메서드
        private HttpResponseMessage CreateErrorResponse(string errorMessage)
        {
            try
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent($"<error>{errorMessage}</error>", Encoding.UTF8, "application/xml");
                return response;
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"오류 응답 생성 실패: {ex.Message}", true);
                // 최소한의 오류 응답 반환
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        // 리소스 정리를 위한 Dispose 메서드
        public void Dispose()
        {
            try
            {
                _client?.Dispose();
                Logger.WriteLog("TaskService 리소스 정리 완료", true);
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"TaskService Dispose 오류: {ex.Message}", true);
            }
        }
    }
}
