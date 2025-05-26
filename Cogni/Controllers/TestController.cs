using Microsoft.AspNetCore.Mvc;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        /// <summary>
        /// ��������� ���� �������� �����
        /// </summary>
        /// <response code="200">������� ����������</response>
        /// <response code="404">� �� ��� �������� ��� ���-�� ���������</response>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _testService.GetAllQuestions();
            if (questions == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(questions);
            }
        }
        /// <summary>
        /// ��������� ������� ����� �� id
        /// </summary>
        /// <response code="200">������� ���������</response>
        /// <response code="404">� �� ��� �������� ��� ���-�� ���������</response>
        [HttpGet]
        public async Task<IActionResult> GetQuestion(QuestionRequest request)
        {
            var question = await _testService.GetById(request.id);
            if (question != null)
            {
                return Ok(question);
            }
            return NotFound();
        }
    }
}