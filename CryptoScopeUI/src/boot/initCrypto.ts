import { boot } from 'quasar/wrappers'
import { useCoinListStore } from 'src/stores/coinListStore'
import { useSearchStore } from 'src/stores/searchStore'

export default boot(async () => {
  const isTestEnv = process.env.VUE_APP_ENV === 'test';
  console.log('[InitCrypto] import.meta.env.VUE_APP_ENV:', process.env.VUE_APP_ENV);

  const coinListStore = useCoinListStore()
  await coinListStore.fetchCoins()

  const searchStore = useSearchStore()
  await searchStore.fetchSearchCoins()
  
  if (isTestEnv) {
    console.log('[InitCrypto] Skipping autorefresh for test environment');
    return;
  }
  else {
    coinListStore.startAutoRefresh(20000)
    searchStore.startAutoRefresh(1000 * 60 * 60 * 24)
  }
})
