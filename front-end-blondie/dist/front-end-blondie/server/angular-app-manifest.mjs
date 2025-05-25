
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
    'index.csr.html': {size: 499, hash: '88967abacd3d024bc21eb14dc11b5e6c68cf22cb9d4393a61f1340afc9842a80', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: '36ed95913a62e9548a1ff8f62948b3c9c937fd3f4a7adc28783350793ad837bc', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10660, hash: '3e3b662bb7d04119ba32b63dbf70aeb79abc4e6fe6f723bb727718ef7bd9c310', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10660, hash: '3e3b662bb7d04119ba32b63dbf70aeb79abc4e6fe6f723bb727718ef7bd9c310', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'documents/index.html': {size: 10660, hash: '3e3b662bb7d04119ba32b63dbf70aeb79abc4e6fe6f723bb727718ef7bd9c310', text: () => import('./assets-chunks/documents_index_html.mjs').then(m => m.default)},
    'index.html': {size: 11050, hash: '7d68a8949644c7b4ea194f9f165e578ec5af77b2a2f0c97e92a04591926ff5d2', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11247, hash: '614f13f2848ce71449d58905359982cc117f545c02be89b06c3a693c385903e4', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'chat/index.html': {size: 8839, hash: 'c87ad37766bb48b3751f0bb726a6d09eb70156c46eec3df185f538b6486d6547', text: () => import('./assets-chunks/chat_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10660, hash: '3e3b662bb7d04119ba32b63dbf70aeb79abc4e6fe6f723bb727718ef7bd9c310', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
