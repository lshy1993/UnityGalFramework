module.exports = {
    base: '/UnityGalFramework/',
    head: [
        ['link', {
            rel: 'icon',
            href: `/favicon.ico`
        }]
    ],
    //dest: '/',
    ga: '',
    evergreen: true,
    themeConfig: {
        repo: 'https://github.com/lshy1993/UnityGalFramework',
        docsDir: 'docs',
        nav: [
            { text: 'Guide', link: '/guide/' },
            { text: 'API', link: '/api/' },
            { text: 'Demo', link: '/demo/' },
            { text: 'Development', link: '/deve/' },
            { text: 'MoeLink', link: 'https://moelink.site' },
        ],
        sidebarDepth: 2,
        sidebar: 'auto'
        //{
            // '/guide/': [{
            //     title: 'Guide',
            //     path: '/guide',
            //     collapsable: false,
            //     children: ['/guide/']
            // }],
            // '/api': [{
            //     title: 'API',
            //     collapsable: false,
            //     children: ['']
            // }],
            // '/demo': [{
            //     title: 'Demos',
            //     collapsable: false,
            //     children: ['']
            // }],
            // '/deve': [{
            //     title: 'Development',
            //     collapsable: false,
            //     children: ['']
            // }]
        //}
    },
    locales: {
        // 键名是该语言所属的子路径
        // 作为特例，默认语言可以使用 '/' 作为其路径。
        '/': {
            lang: 'zh-CN', // 将会被设置为 <html> 的 lang 属性
            title: 'UGF 使用手册',
            description: '文字游戏框架 Unity Galgame Framework',
        },
        '/en/': {
            lang: 'en-US', 
            title: 'UGF Documentation',
            description: 'Unity Galgame Framework, Hello, Galgamer!',
        }
    }
}