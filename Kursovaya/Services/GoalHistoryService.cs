using Microsoft.EntityFrameworkCore;
namespace Kursovaya.Pages
{
    public class GoalHistoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public GoalHistoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddGoalToHistoryAsync(GoalBD goal)
        {
            var existingGoal = await _dbContext.GoalsHistory.FirstOrDefaultAsync(g => g.ID == goal.ID && g.Type == goal.Type && g.Active == goal.Active && g.Weight == goal.Weight && g.Start == goal.Start && g.End == goal.End);

            if (existingGoal != null)
            {
                return true;
            }

            GoalHistory goalHistory = new GoalHistory
            {
                ID = goal.ID,
                Type = goal.Type,
                Active = goal.Active,
                Weight = goal.Weight,
                Start = goal.Start,
                End = goal.End
            };

            _dbContext.GoalsHistory.Add(goalHistory);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<GoalHistory>> GetGoalHistoryAsync(int currentUserId)
        {
            return await _dbContext.GoalsHistory.Where(g => g.ID == currentUserId).ToListAsync();
        }

        public async Task<bool> ClearGoalHistoryAsync(int userId)
        {
            var userGoals = await _dbContext.GoalsHistory.Where(g => g.ID == userId).ToListAsync();

            _dbContext.GoalsHistory.RemoveRange(userGoals);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
