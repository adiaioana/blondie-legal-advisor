
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
    'index.csr.html': {size: 499, hash: '91ae183bf8d06b02940e6cb7944cb7fc0f8cd5dad05285d64269ba8d1f4cd78b', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: '81c44d13475518f626718f844ceffcfc61981eefaef51ffc062eba4d4af10a49', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 11050, hash: 'ff2ed9217baece4dc28d4637202e788f1817a7b2f980c391f3c62c5d423f566a', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10660, hash: 'e7e86dd046e2a8ded2b13db210d94ac5a4b276a910c30e568fe0da96f9c23025', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10660, hash: 'e7e86dd046e2a8ded2b13db210d94ac5a4b276a910c30e568fe0da96f9c23025', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11247, hash: '43a9f840027ef28e3ba8451ab7e8eaf8f840e631be7d4389bafd7286871c65e8', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10660, hash: 'e7e86dd046e2a8ded2b13db210d94ac5a4b276a910c30e568fe0da96f9c23025', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
