
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
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 499, hash: '9629324a7b585f77af57bf813df2511c28dff0eeca41265d9daaba1aecc0bb7e', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: '718479eef0c311413a8c9ac80ed1ea9dcfcca1c0be4c0057557547c10295069a', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 11050, hash: '1908cbc481fb6d40b6c9f6a4b58b346cb1005b5f7f3d9e7c21fd5e1532e1b330', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10660, hash: '611884c1949b6012cf611f2a02eb47ea7110244d23fcb9f1d374759ae4f2780c', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10660, hash: '611884c1949b6012cf611f2a02eb47ea7110244d23fcb9f1d374759ae4f2780c', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10660, hash: '611884c1949b6012cf611f2a02eb47ea7110244d23fcb9f1d374759ae4f2780c', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11247, hash: '5aa9a12f9f4db29d13d3d3ba7bca4e52807ba2c0a9f2e753ab587e0904716933', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
