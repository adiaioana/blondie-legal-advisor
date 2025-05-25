
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
    'index.csr.html': {size: 499, hash: 'f7c992d2a080261c3f62d1c8f8ed4f2eb7849692e9713831f8126b50ef5402ad', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: '5a38198d9ca842ba7d5f0cf66795587136f1ca2493b3d2f52d3b8ac91f105493', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 11077, hash: 'c032916be03f9d819665770001a0077efb9f7105d0432c607697c567916ba771', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10687, hash: 'cac3a5e1cfc556a0296276c9db1138ecbeb64204e7815a267e8d17fb47e314f7', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10687, hash: 'cac3a5e1cfc556a0296276c9db1138ecbeb64204e7815a267e8d17fb47e314f7', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'documents/index.html': {size: 10687, hash: 'cac3a5e1cfc556a0296276c9db1138ecbeb64204e7815a267e8d17fb47e314f7', text: () => import('./assets-chunks/documents_index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10687, hash: 'cac3a5e1cfc556a0296276c9db1138ecbeb64204e7815a267e8d17fb47e314f7', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'chat/index.html': {size: 8866, hash: '27ee860ab08d8f81e0a4ccfd91bc4bf2d00da54fe206c8c1d1f63d4ed55be4fc', text: () => import('./assets-chunks/chat_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11274, hash: 'fae29afc542778d771ae00fbb9a373f184c147c67177706e94e5797506987892', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
