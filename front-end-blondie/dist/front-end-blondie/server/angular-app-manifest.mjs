
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/analyzer"
  },
  {
    "renderMode": 2,
    "route": "/register"
  },
  {
    "renderMode": 2,
    "route": "/login"
  },
  {
    "renderMode": 2,
    "route": "/my-account"
  },
  {
    "renderMode": 2,
    "route": "/documents"
  },
  {
    "renderMode": 1,
    "route": "/documents/*"
  },
  {
    "renderMode": 2,
    "route": "/chat"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 499, hash: '33666da02c2d1d339d6c737e67fa027013e910db8c0e46fe40f46e24b4c3392d', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: '2fc93706d8a717387b7f72819673b97fc83cff9d21f041cf6b44753d5f4d059d', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10660, hash: 'deff40772bb3afa298bae6f2599d53abfe514123b377ef564a7749118bb87f80', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11247, hash: '9e7684797b9c0e277f8062cba2fb0a108661d4a97aa3dabd8da18044a5c4f08e', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10660, hash: 'deff40772bb3afa298bae6f2599d53abfe514123b377ef564a7749118bb87f80', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'documents/index.html': {size: 10660, hash: 'f6f45bbd4238501566a9f7583708d60395647620581ab9bacb8fd2ff9948b78f', text: () => import('./assets-chunks/documents_index_html.mjs').then(m => m.default)},
    'chat/index.html': {size: 8839, hash: '26fe07b61b23a95c8f4d8adb534ac40266274a246bef0deb16093db1686752a6', text: () => import('./assets-chunks/chat_index_html.mjs').then(m => m.default)},
    'index.html': {size: 11050, hash: '311232c6b9dbf3d455891e50bb60d8fa39206a57762c86483e281c88a6d255db', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10660, hash: 'deff40772bb3afa298bae6f2599d53abfe514123b377ef564a7749118bb87f80', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
