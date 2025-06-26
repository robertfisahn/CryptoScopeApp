import { test, expect, _electron as electron } from '@playwright/test';

test('should load coin list in dev Electron app', async () => {
  const electronApp = await electron.launch({
    args: ['.quasar/dev-electron/electron-main.js'],
  });

  const window = await electronApp.firstWindow();

  await window.waitForLoadState('domcontentloaded');

  await window.waitForResponse((resp) =>
    resp.url().includes('/api/coins') && resp.status() === 200
  );

  const coins = await window.locator('[data-testid="coin-item"]');
  await expect(coins).toHaveCount(10);

  await electronApp.close();
});
