/*! modernizr 3.2.0 (Custom Build) | MIT *
 * http://modernizr.com/download/?-fileinput-formattribute-input-inputtypes-shiv !*/
!function(e,t,n){function a(e,t){return typeof e===t}function r(){var e,t,n,r,i,o,c;for(var u in s)if(s.hasOwnProperty(u)){if(e=[],t=s[u],t.name&&(e.push(t.name.toLowerCase()),t.options&&t.options.aliases&&t.options.aliases.length))for(n=0;n<t.options.aliases.length;n++)e.push(t.options.aliases[n].toLowerCase());for(r=a(t.fn,"function")?t.fn():t.fn,i=0;i<e.length;i++)o=e[i],c=o.split("."),1===c.length?Modernizr[c[0]]=r:(!Modernizr[c[0]]||Modernizr[c[0]]instanceof Boolean||(Modernizr[c[0]]=new Boolean(Modernizr[c[0]])),Modernizr[c[0]][c[1]]=r),l.push((r?"":"no-")+c.join("-"))}}function i(e){var t=u.className,n=Modernizr._config.classPrefix||"";if(d&&(t=t.baseVal),Modernizr._config.enableJSClass){var a=new RegExp("(^|\\s)"+n+"no-js(\\s|$)");t=t.replace(a,"$1"+n+"js$2")}Modernizr._config.enableClasses&&(t+=" "+n+e.join(" "+n),d?u.className.baseVal=t:u.className=t)}function o(){return"function"!=typeof t.createElement?t.createElement(arguments[0]):d?t.createElementNS.call(t,"http://www.w3.org/2000/svg",arguments[0]):t.createElement.apply(t,arguments)}var l=[],s=[],c={_version:"3.2.0",_config:{classPrefix:"",enableClasses:!0,enableJSClass:!0,usePrefixes:!0},_q:[],on:function(e,t){var n=this;setTimeout(function(){t(n[e])},0)},addTest:function(e,t,n){s.push({name:e,fn:t,options:n})},addAsyncTest:function(e){s.push({name:null,fn:e})}},Modernizr=function(){};Modernizr.prototype=c,Modernizr=new Modernizr;var u=t.documentElement,d="svg"===u.nodeName.toLowerCase();d||!function(e,t){function n(e,t){var n=e.createElement("p"),a=e.getElementsByTagName("head")[0]||e.documentElement;return n.innerHTML="x<style>"+t+"</style>",a.insertBefore(n.lastChild,a.firstChild)}function a(){var e=b.elements;return"string"==typeof e?e.split(" "):e}function r(e,t){var n=b.elements;"string"!=typeof n&&(n=n.join(" ")),"string"!=typeof e&&(e=e.join(" ")),b.elements=n+" "+e,c(t)}function i(e){var t=y[e[g]];return t||(t={},v++,e[g]=v,y[v]=t),t}function o(e,n,a){if(n||(n=t),d)return n.createElement(e);a||(a=i(n));var r;return r=a.cache[e]?a.cache[e].cloneNode():h.test(e)?(a.cache[e]=a.createElem(e)).cloneNode():a.createElem(e),!r.canHaveChildren||p.test(e)||r.tagUrn?r:a.frag.appendChild(r)}function l(e,n){if(e||(e=t),d)return e.createDocumentFragment();n=n||i(e);for(var r=n.frag.cloneNode(),o=0,l=a(),s=l.length;s>o;o++)r.createElement(l[o]);return r}function s(e,t){t.cache||(t.cache={},t.createElem=e.createElement,t.createFrag=e.createDocumentFragment,t.frag=t.createFrag()),e.createElement=function(n){return b.shivMethods?o(n,e,t):t.createElem(n)},e.createDocumentFragment=Function("h,f","return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&("+a().join().replace(/[\w\-:]+/g,function(e){return t.createElem(e),t.frag.createElement(e),'c("'+e+'")'})+");return n}")(b,t.frag)}function c(e){e||(e=t);var a=i(e);return!b.shivCSS||u||a.hasCSS||(a.hasCSS=!!n(e,"article,aside,dialog,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}mark{background:#FF0;color:#000}template{display:none}")),d||s(e,a),e}var u,d,m="3.7.3",f=e.html5||{},p=/^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i,h=/^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i,g="_html5shiv",v=0,y={};!function(){try{var e=t.createElement("a");e.innerHTML="<xyz></xyz>",u="hidden"in e,d=1==e.childNodes.length||function(){t.createElement("a");var e=t.createDocumentFragment();return"undefined"==typeof e.cloneNode||"undefined"==typeof e.createDocumentFragment||"undefined"==typeof e.createElement}()}catch(n){u=!0,d=!0}}();var b={elements:f.elements||"abbr article aside audio bdi canvas data datalist details dialog figcaption figure footer header hgroup main mark meter nav output picture progress section summary template time video",version:m,shivCSS:f.shivCSS!==!1,supportsUnknownElements:d,shivMethods:f.shivMethods!==!1,type:"default",shivDocument:c,createElement:o,createDocumentFragment:l,addElements:r};e.html5=b,c(t),"object"==typeof module&&module.exports&&(module.exports=b)}("undefined"!=typeof e?e:this,t),Modernizr.addTest("fileinput",function(){if(navigator.userAgent.match(/(Android (1.0|1.1|1.5|1.6|2.0|2.1))|(Windows Phone (OS 7|8.0))|(XBLWP)|(ZuneWP)|(w(eb)?OSBrowser)|(webOS)|(Kindle\/(1.0|2.0|2.5|3.0))/))return!1;var e=o("input");return e.type="file",!e.disabled}),Modernizr.addTest("formattribute",function(){var e,n=o("form"),a=o("input"),r=o("div"),i="formtest"+(new Date).getTime(),l=!1;n.id=i;try{a.setAttribute("form",i)}catch(s){t.createAttribute&&(e=t.createAttribute("form"),e.nodeValue=i,a.setAttributeNode(e))}return r.appendChild(n),r.appendChild(a),u.appendChild(r),l=n.elements&&1===n.elements.length&&a.form==n,r.parentNode.removeChild(r),l});var m=o("input"),f="search tel url email datetime date month week time datetime-local number range color".split(" "),p={};Modernizr.inputtypes=function(e){for(var a,r,i,o=e.length,l=":)",s=0;o>s;s++)m.setAttribute("type",a=e[s]),i="text"!==m.type&&"style"in m,i&&(m.value=l,m.style.cssText="position:absolute;visibility:hidden;",/^range$/.test(a)&&m.style.WebkitAppearance!==n?(u.appendChild(m),r=t.defaultView,i=r.getComputedStyle&&"textfield"!==r.getComputedStyle(m,null).WebkitAppearance&&0!==m.offsetHeight,u.removeChild(m)):/^(search|tel)$/.test(a)||(i=/^(url|email|number)$/.test(a)?m.checkValidity&&m.checkValidity()===!1:m.value!=l)),p[e[s]]=!!i;return p}(f);var h="autocomplete autofocus list placeholder max min multiple pattern required step".split(" "),g={};Modernizr.input=function(t){for(var n=0,a=t.length;a>n;n++)g[t[n]]=!!(t[n]in m);return g.list&&(g.list=!(!o("datalist")||!e.HTMLDataListElement)),g}(h),r(),i(l),delete c.addTest,delete c.addAsyncTest;for(var v=0;v<Modernizr._q.length;v++)Modernizr._q[v]();e.Modernizr=Modernizr}(window,document);