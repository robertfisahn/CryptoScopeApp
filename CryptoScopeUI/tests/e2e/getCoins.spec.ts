import { test, expect } from '@playwright/test';

test('should show coin list when data exists', async ({ page, request }) => {
  // Resetuj dane
  await request.post('http://localhost:5888/api/test/reset');

  await page.goto('http://localhost:9000'); // baseURL jest już ustawione

  await page.waitForResponse(resp =>
    resp.url().includes('http://localhost:5888/api/test/coins') && resp.status() === 200
  );

  const coins = page.locator('[data-testid="coin-item"]');
  await expect(coins).toHaveCount(2); // lub tyle ile daje seeder
});

test('should show empty state when no coins exist', async ({ page, request }) => {
  await request.post('http://localhost:5888/api/test/reset');
  await request.delete('http://localhost:5888/api/test/coins');

  await page.goto('http://localhost:9000');

  const coins = page.locator('[data-testid="coin-item"]');
  await expect(coins).toHaveCount(0);

  await expect(page.locator('[data-testid="empty-state"]')).toBeVisible();
});
