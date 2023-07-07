<template>
    <!--<img alt="Vue logo" src="./assets/logo.png">-->

    <navbar-component
        :pages="pages"
        :active-page="activePage"
        :nav-link-click="(index) => activePage = index"
    ></navbar-component>
    <page-content
        :page="pages[activePage]"
    ></page-content>

    <counter-button />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import NavbarComponent from './components/NavbarComponent.vue';
import PageContent from './components/PageContent.vue';
import CounterButton from './components/CounterButton.vue';

    export default defineComponent({
        components: {
            NavbarComponent,
            PageContent,
            CounterButton
        },
        data() {
            return {
                activePage: 0,
                pages: [
                    {
                        link: { text: "Home", url: "index.html" },
                        pageTitle: "Home Page",
                        content: "This is the home content."
                    },
                    {
                        link: { text: "About", url: "about.html" },
                        pageTitle: "About Page",
                        content: "This is the about content."

                    },
                    {
                        link: { text: "Contact", url: "contact.html" },
                        pageTitle: "Contact Page",
                        content: "This is the contact content."
                    }
                ]
            }
        }
    })
</script>
