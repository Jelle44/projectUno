<template>
    <nav :class="['navbar', 'navbar-expand-lg']">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">Uno</a>
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                <li v-for="(page, index) in pages" class="nav-item" :key="index">
                    <navbar-link
                        :page="page"
                        :isActive="activePage === index"
                        @click.prevent="navLinkClick(index)"
                    ></navbar-link>
                </li>
            </ul>
            <form class="d-flex">
                <button
                    class="btn btn-primary"
                    @click.prevent="changeTheme()"
                >Change theme</button>
            </form>
        </div>
    </nav>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import NavbarLink from './NavbarLink.vue';

    export default defineComponent({
        components: {
            NavbarLink
        },
        props: ['pages', 'activePage', 'navLinkClick'],
        methods: {
            changeTheme() {
                let theme = document.body.getAttribute('data-bs-theme');

                if(theme == 'light'){
                    theme = 'dark';
                } else {
                    theme = 'light';
                }

                document.body.setAttribute('data-bs-theme', theme);
            }
        }
    })
</script>
